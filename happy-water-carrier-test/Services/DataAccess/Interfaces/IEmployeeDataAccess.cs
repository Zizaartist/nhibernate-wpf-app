using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using System.Collections.Generic;

namespace happy_water_carrier_test.Services.DataAccess
{
    public interface IEmployeeDataAccess
    {
        void Add(Employee newEmployee);
        Employee Get(int id);
        IEnumerable<ListElementModel> GetIdList();
        IEnumerable<ListElementModel> GetSubdivisionsIdList();
        void Remove(int id);
        void SaveChanges();
        void Update(Employee updatedEmployee);
    }
}