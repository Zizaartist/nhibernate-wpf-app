using System;
using System.Collections.Generic;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class Subdivision
    {
        public Subdivision()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? DirectorId { get; set; }

        public virtual Employee Director { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
