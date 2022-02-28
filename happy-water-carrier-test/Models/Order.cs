using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class Order
    {
        public Order()
        {
            Tags = new HashSet<Tag>();
            OrderTags = new HashSet<OrderTag>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<OrderTag> OrderTags { get; set; }
    }
}
