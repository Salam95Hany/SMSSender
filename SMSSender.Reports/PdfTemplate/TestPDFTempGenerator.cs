using Microsoft.AspNetCore.Hosting;
using RazorLight;
using SMSSender.Entities.Reports;
using SMSSender.Reports.Interface;
using SMSSender.Reports.Model;
using SMSSender.Reports.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.PdfTemplate
{
    public class TestPDFTempGenerator : ReportGenerator, IReportGenerator
    {
        private readonly IWebHostEnvironment _environment;
        public override ReportType ReportType => ReportType.DischargeSummary;

        public TestPDFTempGenerator(IWebHostEnvironment environment, IRazorLightEngine razorEngine, IPDFHelper pDFHelper) : base(razorEngine, pDFHelper)
        {
            _environment = environment;
        }


        public async Task<string> Generate(SearchReportModel Model)
        {
            var PatientId = Model.QueryString.FirstOrDefault(i => i.Key == "PatientId")?.Value;
            var AdmissionId = Model.QueryString.FirstOrDefault(i => i.Key == "AdmissionId")?.Value;
            var SurgicalId = Model.QueryString.FirstOrDefault(i => i.Key == "SurgicalId")?.Value;
            if (!string.IsNullOrEmpty(PatientId) && !string.IsNullOrEmpty(AdmissionId) && !string.IsNullOrEmpty(SurgicalId))
            {
                var Results = new DataTable(); //await _reportsDataService.GetAdmissionTempData(int.Parse(PatientId), int.Parse(AdmissionId), int.Parse(SurgicalId));
                var Data = new PdfDataReports
                {
                    Data = new Dictionary<string, string>(),
                    ImageSrc = Path.Combine(_environment.WebRootPath, "Template", "Logo.png")
                };

                var FullPath = await this.Build(Data);
                return FullPath;
            }
            else
                return string.Empty;

        }
    }
}
