using System;
using System.Collections.Generic;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class OrderTag
    {
        public int OrderId { get; set; }
        public int TagId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
