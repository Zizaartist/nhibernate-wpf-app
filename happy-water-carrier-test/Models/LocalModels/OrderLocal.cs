using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace happy_water_carrier_test.Models.LocalModels
{
    public class OrderLocal : ObservableValidator
    {
        public OrderLocal()
        {
            IncludedTags = new ObservableCollection<ListElementModel>();
        }

        public int Id { get; set; }

        private string productName;
        [MaxLength(100)]
        public string ProductName
        {
            get => productName;
            set => SetProperty(ref productName, value, true);
        }

        private ListElementModel employee;
        public ListElementModel Employee
        {
            get => employee;
            set => SetProperty(ref employee, value);
        }

        public ObservableCollection<ListElementModel> IncludedTags { get; }
    }
}
