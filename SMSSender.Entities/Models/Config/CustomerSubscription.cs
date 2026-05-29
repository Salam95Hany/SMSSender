using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Config
{
    [Table(name: "CustomerSubscriptions", Schema = "config")]
    public class CustomerSubscription
    {
        public int CustomerSubscriptionId { get; set; }
        public Guid CustomerId { get; set; }
        public int PlanId { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
