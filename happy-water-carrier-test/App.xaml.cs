using Autofac;
using Autofac.Extensions.DependencyInjection;
using happy_water_carrier_test.Helpers;
using happy_water_carrier_test.Services.DataAccess;
using happy_water_carrier_test.ViewModels;
using happy_water_carrier_test.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace happy_water_carrier_test
{
    public partial class App : Application
    {
        private IConfiguration Configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            var confBuilder = new ConfigurationBuilder();
            var autofacBuilder = new ContainerBuilder();

            confBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            Configuration = confBuilder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            autofacBuilder.Populate(services);
            ConfigurePages(autofacBuilder);
            BootStrapper.Start(autofacBuilder);

            var window = BootStrapper.Resolve<MainWindow>();
            window.Show();
        }

        private void ConfigurePages(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<EmployeePage>().AsSelf();
            builder.RegisterType<SubdivisionPage>().AsSelf();
            builder.RegisterType<OrderPage>().AsSelf();

            builder.RegisterType<EmployeeViewModel>().AsSelf();
            builder.RegisterType<SubdivisionViewModel>().AsSelf();
            builder.RegisterType<OrderViewModel>().AsSelf();

            builder.RegisterType<EmployeeDataAccess>().As<IEmployeeDataAccess>();
            builder.RegisterType<SubdivisionDataAccess>().As<ISubdivisionDataAccess>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var connStr = Configuration.GetConnectionString("RemoteConnection");
            services.AddDbContext<DBContext>(options => 
            {
                options.UseSqlServer(connStr);
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            BootStrapper.Stop();
        }
    }
}
