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
    /// Логика взаимодействия для SubdivisionPage.xaml
    /// </summary>
    public partial class SubdivisionPage : Page
    {
        public SubdivisionPage(SubdivisionViewModel subdivisionViewModel)
        {
            InitializeComponent();
            this.DataContext = subdivisionViewModel;
            subdivisionViewModel.Refresh.Execute(null);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
