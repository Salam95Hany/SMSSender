using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Config
{
    [Table(name: "Branches", Schema = "config")]
    public class Branch
    {
        public int BranchId { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsMain { get; set; } // الفرع الرئيسي
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
