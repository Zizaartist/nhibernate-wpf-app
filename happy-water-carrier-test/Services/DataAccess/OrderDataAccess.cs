using happy_water_carrier_test.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace happy_water_carrier_test.Services.DataAccess
{
    public class OrderDataAccess
    {
        private readonly DBContext _context;

        public OrderDataAccess(DBContext context)
        {
            this._context = context;
        }

        public IEnumerable<KeyValuePair<string, int>> GetIdList() =>
            _context.Employees.Select(em =>
                new KeyValuePair<string, int>($"{em.FirstName} {em.LastName} {em.PatronymicName}", em.Id));

        public IEnumerable<KeyValuePair<string, int>> GetEmployeesIdList() =>
            _context.Subdivisions.Select(su =>
                new KeyValuePair<string, int>(su.Name, su.Id));

        // TO-DO replace, also replace intermediate model in order and tag
        //public IEnumerable<KeyValuePair<string, int>> GetRelatedTagsIdList(int orderId) =>
        //    _context.Orders.Include(or => or.OrderTags);

        //public IEnumerable

        public Order Get(int id) => _context.Orders.Find(id);

        public void Add(Order newEmployee) => _context.Orders.Add(newEmployee);

        public void Update(Order updatedEmployee) => _context.Orders.Update(updatedEmployee);

        public void Remove(Order toRemove) => _context.Orders.Remove(toRemove);

        public void SaveChanges() => _context.SaveChanges();
    }
}
