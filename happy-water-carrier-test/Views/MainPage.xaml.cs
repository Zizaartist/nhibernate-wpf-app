using happy_water_carrier_test.Helpers;
using System.Windows.Controls;

namespace happy_water_carrier_test.Views
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void EmployeeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var employeePage = BootStrapper.Resolve<EmployeePage>();
            NavigationService.Navigate(employeePage);
        }

        private void SubdivisionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var subdivisionPage = BootStrapper.Resolve<SubdivisionPage>();
            NavigationService.Navigate(subdivisionPage);
        }

        private void OrderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var orderPage = BootStrapper.Resolve<OrderPage>();
            NavigationService.Navigate(orderPage);
        }
    }
}
