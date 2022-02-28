using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace happy_water_carrier_test.Services.DataAccess
{
    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly DBContext _context;

        public OrderDataAccess(DBContext context)
        {
            this._context = context;
        }

        public IEnumerable<ListElementModel> GetIdList() =>
            _context.Orders
                .AsNoTracking()
                .Select(em => new ListElementModel
                {
                    Id = em.Id,
                    Value = em.ProductName
                });

        public IEnumerable<ListElementModel> GetEmployeesIdList() =>
            _context.Employees
                .AsNoTracking()
                .Select(su => new ListElementModel
                {
                    Id = su.Id,
                    Value = $"{su.LastName} {su.FirstName} {su.PatronymicName}"
                });

        public IEnumerable<ListElementModel> GetTagsIdList() =>
            _context.Tags
                .AsNoTracking()
                .Select(tag => new ListElementModel
                {
                    Id = tag.Id,
                    Value = tag.Name
                });

        public Order Get(int id) => 
            _context.Orders
                .AsNoTracking()
                .Include(em => em.Tags)
                .FirstOrDefault(em => em.Id == id);



        public void Add(Order newOrder) 
        {
            _context.AttachRange(newOrder.Tags);
            foreach (var tag in newOrder.Tags)
                _context.Entry(tag).State = EntityState.Unchanged;
            _context.Orders.Add(newOrder);
        }

        public void Update(Order updatedOrder)
        {
            var newIdsList = updatedOrder.Tags.Select(tag => tag.Id).ToList();
            updatedOrder.Tags.Clear();
            _context.Attach(updatedOrder);
            _context.Entry(updatedOrder).Collection(uo => uo.Tags).Load();
            var currentIdsList = updatedOrder.Tags.Select(tag => tag.Id).ToList();

            var toAdd = newIdsList.Where(nid => !currentIdsList.Any(cid => cid == nid));
            var toRemoveList = currentIdsList.Where(cid => !newIdsList.Any(nid => nid == cid));
            foreach (var tagId in toAdd)
            {
                var addedTag = new Tag { Id = tagId };
                _context.Attach(addedTag);
                updatedOrder.Tags.Add(addedTag);
            }
            foreach (var tagId in toRemoveList)
            {
                var toRemove = updatedOrder.Tags.First(tag => tag.Id == tagId);
                updatedOrder.Tags.Remove(toRemove);
            }

            _context.Orders.Update(updatedOrder);
        }

        public void Remove(int id)
        {
            var Order = new Order { Id = id };
            _context.Attach(Order);
            _context.Remove(Order);
        }

        public void AddTag(Tag newTag) => _context.Tags.Add(newTag);

        public void UpdateTag(Tag updatedTag) => _context.Tags.Update(updatedTag);

        public void RemoveTag(int id)
        {
            var tag = new Tag { Id = id };
            _context.Attach(tag);
            _context.Remove(tag);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
