using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using System.Collections.Generic;

namespace happy_water_carrier_test.Services.DataAccess
{
    public interface IOrderDataAccess
    {
        void Add(Order newOrder);
        void AddTag(Tag newTag);
        Order Get(int id);
        IEnumerable<ListElementModel> GetEmployeesIdList();
        IEnumerable<ListElementModel> GetIdList();
        IEnumerable<ListElementModel> GetTagsIdList();
        void Remove(int id);
        void RemoveTag(int id);
        void SaveChanges();
        void Update(Order updatedOrder);
        void UpdateTag(Tag updatedTag);
    }
}