using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happy_water_carrier_test.Services.DataAccess
{
    public class SubdivisionDataAccess : ISubdivisionDataAccess
    {
        private readonly DBContext _context;

        public SubdivisionDataAccess(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<ListElementModel> GetIdList() =>
            _context.Subdivisions
                .AsNoTracking()
                .Select(em => new ListElementModel
                {
                    Id = em.Id,
                    Value = em.Name
                });

        public IEnumerable<ListElementModel> GetEmployeesIdList() =>
            _context.Employees
                .AsNoTracking()
                .Select(em => new ListElementModel
                {
                    Id = em.Id,
                    Value = $"{em.FirstName} {em.LastName} {em.PatronymicName}"
                });

        public Subdivision Get(int id) =>
            _context.Subdivisions
                .AsNoTracking()
                .FirstOrDefault(su => su.Id == id);

        public void Add(Subdivision newSubdivision) => _context.Subdivisions.Add(newSubdivision);

        public void Update(Subdivision updatedSubdivision) => _context.Subdivisions.Update(updatedSubdivision);

        public void Remove(int id)
        {
            var subdivision = new Subdivision { Id = id };
            _context.Attach(subdivision);
            _context.Remove(subdivision);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
