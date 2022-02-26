using happy_water_carrier_test.Models.Enum;
using System;
using System.Collections.Generic;

#nullable disable

namespace happy_water_carrier_test.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public int? SubdivisionId { get; set; }

        public virtual Subdivision Subdivision { get; set; }
        public virtual Subdivision SubdivisionNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
