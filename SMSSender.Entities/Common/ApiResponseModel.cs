using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Common
{
    public class ApiResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int TotalCount { get; set; }
        public T? Results { get; set; }

        public static ApiResponseModel<T> Success(Error SuccessError, T? value = default, int? TotalCount = null)
        {
            return new ApiResponseModel<T>
            {
                IsSuccess = true,
                Message = SuccessError.Message,
                TotalCount = TotalCount ?? GetCount(value),
                Results = value
            };
        }

        public static ApiResponseModel<T> Failure(Error Error)
        {
            return new ApiResponseModel<T>
            {
                IsSuccess = false,
                Message = Error.Message,
                TotalCount = 0,
                Results = default
            };
        }

        private static int GetCount(T? value)
        {
            if (value == null)
                return 0;

            if (value is DataSet dataSet)
                return GetCountFromDataSet(dataSet);

            if (value is DataTable dataTable)
                return GetCountFromDataTable(dataTable);

            if (value is ICollection collection)
                return collection.Count;

            if (value is IEnumerable enumerable)
                return enumerable.Cast<object>().Count();

            return 0;
        }

        private static int GetCountFromDataSet(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
                return 0;

            foreach (DataTable table in dataSet.Tables)
            {
                if (table == null || table.Rows.Count == 0)
                    continue;

                var totalCount = TryGetTotalCount(table);
                if (totalCount.HasValue)
                    return totalCount.Value;
            }

            foreach (DataTable table in dataSet.Tables)
            {
                if (table != null && table.Rows.Count > 0)
                    return table.Rows.Count;
            }

            return 0;
        }

        private static int GetCountFromDataTable(DataTable dataTable)
        {
            if (dataTable == null)
                return 0;

            var totalCount = TryGetTotalCount(dataTable);
            if (totalCount.HasValue)
                return totalCount.Value;

            return dataTable.Rows.Count;
        }

        private static int? TryGetTotalCount(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return null;

            var totalCountColumn = table.Columns.Cast<DataColumn>().FirstOrDefault(c => string.Equals(c.ColumnName, "TotalCount", StringComparison.OrdinalIgnoreCase));

            if (totalCountColumn == null)
                return null;

            var firstValue = table.Rows[0][totalCountColumn];
            if (firstValue == null || firstValue == DBNull.Value)
                return null;

            if (int.TryParse(firstValue.ToString(), out int totalCount))
                return totalCount;

            return null;
        }
    }
}
