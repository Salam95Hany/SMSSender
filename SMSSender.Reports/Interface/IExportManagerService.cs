using SMSSender.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Reports.Interface
{
    public interface IExportManagerService
    {
        string Export(ExportTemplateBase exportTemplateBase, DataTable data);
        List<string> GetMainSurgeons(string filePath);
        List<(string DoctorName, string AcademicDegree)> GetDoctorsWithRoles(string filePath);
    }
}
