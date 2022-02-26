using System;
using System.Collections.Generic;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderTags = new HashSet<OrderTag>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<OrderTag> OrderTags { get; set; }
    }
}
