using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Config
{
    [Table(name: "Plans", Schema = "config")]
    public class Plan
    {
        public int PlanId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int MaxUsers { get; set; }
        public bool IsActive { get; set; }
    }
}
