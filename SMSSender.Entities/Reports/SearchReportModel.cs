using Microsoft.AspNetCore.Http;
using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Reports
{
    public class SearchReportModel
    {
        public string ReportType { get; set; }
        public string? UserName { get; set; }
        public List<QueryString> QueryString { get; set; } = new();
        public List<FilterModel> FilterList { get; set; } = new();
    }

    public class QueryString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
