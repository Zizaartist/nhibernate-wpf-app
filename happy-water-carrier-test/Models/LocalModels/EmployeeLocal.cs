using CommunityToolkit.Mvvm.ComponentModel;
using happy_water_carrier_test.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace happy_water_carrier_test.Models.LocalModels
{
    public class EmployeeLocal : ObservableValidator
    {
        public int Id { get; set; }

        private string firstName;
        [MaxLength(100)]
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value, true);
        }

        private string lastName;
        [MaxLength(100)]
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value, true);
        }

        private string patronymicName;
        [MaxLength(100)]
        public string PatronymicName
        {
            get => patronymicName;
            set => SetProperty(ref patronymicName, value, true);
        }

        private DateTime? dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => dateOfBirth;
            set => SetProperty(ref dateOfBirth, value, true);
        }

        private int gender;
        public int Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }

        private ListElementModel subdivision;
        public ListElementModel Subdivision
        {
            get => subdivision;
            set => SetProperty(ref subdivision, value);
        }
    }
}
