using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace happy_water_carrier_test.Services.DataAccess
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {
        private readonly DBContext _context;

        public EmployeeDataAccess(DBContext context)
        {
            this._context = context;
        }

        public IEnumerable<ListElementModel> GetIdList() =>
            _context.Employees
                .AsNoTracking()
                .Select(em => new ListElementModel
                    {
                        Id = em.Id,
                        Value = $"{em.FirstName} {em.LastName} {em.PatronymicName}"
                    });

        public IEnumerable<ListElementModel> GetSubdivisionsIdList() =>
            _context.Subdivisions
                .AsNoTracking()
                .Select(su => new ListElementModel 
                    {
                        Id = su.Id,
                        Value = su.Name
                    });

        public Employee Get(int id) => 
            _context.Employees
                .AsNoTracking()
                .FirstOrDefault(em => em.Id == id);

        public void Add(Employee newEmployee) => _context.Employees.Add(newEmployee);

        public void Update(Employee updatedEmployee) => _context.Employees.Update(updatedEmployee);

        public void Remove(int id) 
        {
            var employee = new Employee { Id = id };
            _context.Attach(employee);
            _context.Remove(employee);
        }

        public void SaveChanges() 
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
