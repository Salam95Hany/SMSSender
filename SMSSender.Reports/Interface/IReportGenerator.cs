using SMSSender.Entities.Reports;
using SMSSender.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.Interface
{
    public interface IReportGenerator
    {
        ReportType ReportType { get; }
        Task<string> Generate(SearchReportModel Model);
    }
}
