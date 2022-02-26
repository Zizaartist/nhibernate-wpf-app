using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using System.Collections.Generic;

namespace happy_water_carrier_test.Services.DataAccess
{
    public interface ISubdivisionDataAccess
    {
        void Add(Subdivision newSubdivision);
        Subdivision Get(int id);
        IEnumerable<ListElementModel> GetEmployeesIdList();
        IEnumerable<ListElementModel> GetIdList();
        void Remove(int id);
        void SaveChanges();
        void Update(Subdivision updatedSubdivision);
    }
}