using SMSSender.Entities.Reports;
using SMSSender.Reports.Interface;
using SMSSender.Reports.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.ExcelTemplate
{
    public class TestExcelTempGenerator : IReportGenerator
    {
        private readonly IExportManagerService _exportManagerService;
        public TestExcelTempGenerator(IExportManagerService exportManagerService)
        {
            _exportManagerService = exportManagerService;
        }

        public ReportType ReportType => ReportType.PatientSearchExcel;

        public async Task<string> Generate(SearchReportModel Model)
        {
            var Data = new DataTable();//await _patientSearchService.GetExportPatientSearchData(Model.FilterList);
            var ExportTemplate = new ExportTemplateBase { Name = "Patient Search", SheetName = "Patient Search", TemplateName = "Patient Search", UserName = Model.UserName };
            var File = _exportManagerService.Export(ExportTemplate, Data);
            return File;
        }
    }
}
