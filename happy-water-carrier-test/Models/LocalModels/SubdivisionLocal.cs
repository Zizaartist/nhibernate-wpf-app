using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace happy_water_carrier_test.Models.LocalModels
{
    public class SubdivisionLocal : ObservableValidator
    {
        public int Id { get; set; }

        private string name;
        [MaxLength(100)]
        public string Name 
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        private ListElementModel director;
        public ListElementModel Director 
        {
            get => director;
            set => SetProperty(ref director, value);
        }
    }
}
