using happy_water_carrier_test.Models.LocalModels;
using happy_water_carrier_test.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace happy_water_carrier_test.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            DataContext = orderViewModel;
            orderViewModel.Refresh.Execute(null);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private int? lastSelectedTagId;

        private void ExcludedTagsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItemObj = (e.AddedItems as IEnumerable<object> ?? Enumerable.Empty<object>()).FirstOrDefault();
            if (selectedItemObj != null)
            {
                var selectedItem = selectedItemObj as ListElementModel;
                if (lastSelectedTagId != selectedItem.Id)
                    TagNameField.Text = selectedItem.Value;
                lastSelectedTagId = selectedItem.Id;
            }
            else
                lastSelectedTagId = null;
        }
    }
}
