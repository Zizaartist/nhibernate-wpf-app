using System;
using System.Collections.Generic;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class Tag
    {
        public Tag()
        {
            OrderTags = new HashSet<OrderTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderTag> OrderTags { get; set; }
    }
}
