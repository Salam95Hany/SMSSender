using SMSSender.Reports.Interface;
using SMSSender.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.Service
{
    public class ReportGeneratorFactory: IReportGeneratorFactory
    {
        private readonly Dictionary<ReportType, IReportGenerator> _generators;

        public ReportGeneratorFactory(IEnumerable<IReportGenerator> generators)
        {
            _generators = generators.ToDictionary(g => g.ReportType, g => g);
        }

        public IReportGenerator GetGenerator(ReportType type)
        {
            if (!_generators.TryGetValue(type, out var generator))
                throw new NotSupportedException($"Unsupported report type: {type}");

            return generator;
        }
    }
}
