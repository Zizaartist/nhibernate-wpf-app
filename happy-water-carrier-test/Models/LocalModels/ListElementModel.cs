using CommunityToolkit.Mvvm.ComponentModel;

namespace happy_water_carrier_test.Models.LocalModels
{
    public class ListElementModel : ObservableObject
    {
        private int id;
        public int Id 
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string val;
        public string Value 
        {
            get => val;
            set => SetProperty(ref val, value);
        }
    }
}
