using RazorLight;
using SMSSender.Reports.Interface;
using SMSSender.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.Service
{
    public abstract class ReportGenerator
    {
        public abstract ReportType ReportType { get; }
        private readonly IRazorLightEngine _razorEngine;
        private readonly IPDFHelper _pDFHelper;

        protected ReportGenerator(IRazorLightEngine razorEngine, IPDFHelper pDFHelper)
        {
            _razorEngine = razorEngine;
            _pDFHelper = pDFHelper;
        }

        public async Task<string> Build(object Model)
        {
            try
            {
                var html = await _razorEngine.CompileRenderAsync(ReportType.ToString(), Model);
                var FilePath = _pDFHelper.SaveHTMLResult(html);
                return FilePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
