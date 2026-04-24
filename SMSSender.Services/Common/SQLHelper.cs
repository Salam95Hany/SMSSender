using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SMSSender.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services.Common
{
    public class SQLHelper : ISQLHelper
    {
        private readonly IAppSettings _appSettings;
        private readonly int _timeout = 9999;
        private readonly string _connectionString;

        public SQLHelper(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _connectionString = appSettings.ConnectionStrings.DBConnection;
        }

        public async Task<List<TElement>> SQLQueryAsync<TElement>(string commandText, params SqlParameter[] parameters)
        {
            return await ExecuteReaderAsync(commandText, reader =>
            {
                var result = MapToList<TElement>(reader);
                return Task.FromResult(result);
            }, parameters);
        }

        public async Task<DataTable> ExecuteDataTableAsync(string commandText, params SqlParameter[] parameters)
        {
            return await ExecuteReaderAsync(commandText, reader =>
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return Task.FromResult(dataTable);
            }, parameters);
        }

        public async Task<TResult> ExecuteReaderAsync<TResult>(string commandText, Func<DbDataReader, Task<TResult>> handler, params SqlParameter[] parameters)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = CreateStoredProcedureCommand(connection, commandText, parameters))
                using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                {
                    return await handler(reader);
                }
            }
        }

        public async Task<DataSet> ExecuteDatasetAsync(string commandText, SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = CreateStoredProcedureCommand(connection, commandText, commandParameters))
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Load(reader, LoadOption.PreserveChanges, new[] { "Table", "Table1", "Table2", "Table3" });
                    return dataSet;
                }
            }
        }

        public async Task<int> ExecuteScalarAsync(string procName, params SqlParameter[] sqlParameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (SqlCommand command = CreateStoredProcedureCommand(connection, procName, sqlParameters))
            {
                await connection.OpenAsync();

                object result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public async Task<int> GenerateCode(string procName)
        {
            return await ExecuteScalarAsync(procName, Array.Empty<SqlParameter>());
        }

        private SqlCommand CreateStoredProcedureCommand(SqlConnection connection, string commandText, SqlParameter[] parameters)
        {
            var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = _timeout,
                CommandType = CommandType.StoredProcedure
            };

            AttachParameters(command, CloneParameters(parameters));
            return command;
        }

        private static SqlParameter[] CloneParameters(IEnumerable<SqlParameter>? parameters)
        {
            if (parameters == null)
            {
                return Array.Empty<SqlParameter>();
            }

            return parameters.Select(parameter =>
            {
                var clone = (SqlParameter)((ICloneable)parameter).Clone();
                if (clone.Value == null)
                {
                    clone.Value = DBNull.Value;
                }

                return clone;
            }).ToArray();
        }

        private static void AttachParameters(SqlCommand command, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        private List<T> MapToList<T>(DbDataReader reader)
        {
            var objectList = new List<T>();
            var properties = typeof(T).GetRuntimeProperties().ToArray();
            var columnNames = Enumerable.Range(0, reader.FieldCount)
                .Select(reader.GetName)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (!reader.HasRows)
            {
                return objectList;
            }

            while (reader.Read())
            {
                T instance = Activator.CreateInstance<T>();

                foreach (var property in properties)
                {
                    if (!columnNames.Contains(property.Name))
                    {
                        continue;
                    }

                    var ordinal = reader.GetOrdinal(property.Name);
                    var value = reader.GetValue(ordinal);
                    property.SetValue(instance, value == DBNull.Value ? null : value);
                }

                objectList.Add(instance);
            }

            return objectList;
        }
    }
}
