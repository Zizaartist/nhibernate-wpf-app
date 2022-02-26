using CommunityToolkit.Mvvm.ComponentModel;
using happy_water_carrier_test.Models.LocalModels;
using System.Collections.ObjectModel;

namespace happy_water_carrier_test.ViewModels
{
    public class OrderViewModel : ObservableObject
    {
        public ObservableCollection<ListElementModel> Orders { get; } = new ObservableCollection<ListElementModel>();

        public OrderViewModel()
        {
            
        }
    }
}