using SMSSender.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.Interface
{
    public interface IReportGeneratorFactory
    {
        IReportGenerator GetGenerator(ReportType type);
    }
}
