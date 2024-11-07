using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueDataBaseImplement;
using ClinicQueueDataBaseImplement.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace ClinicQueueView
{
    public partial class App : Application
    {
        private static ServiceProvider? _serviceProvider;
        public static ServiceProvider? ServiceProvider => _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при открытии MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            base.OnStartup(e);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Регистрация логирования
            services.AddLogging(configure => configure.AddConsole());

            // Регистрация зависимостей
            services.AddTransient<IAdminStorage, AdminStorage>();
            services.AddTransient<IDoctorStorage, DoctorStorage>();
            services.AddTransient<IAdminLogic, AdminLogic>();
            services.AddTransient<IDoctorLogic, DoctorLogic>();

            // Регистрация MainWindow с внедрением зависимостей
            services.AddTransient<MainWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<AddDoctorWindow>();
            services.AddTransient<EditDoctorWindow>();
        }
    }
}
