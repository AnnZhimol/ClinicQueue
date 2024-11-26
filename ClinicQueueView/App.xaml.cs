using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Implements;
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
                var startWindow = _serviceProvider.GetRequiredService<StartWindow>();
                startWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при открытии MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            base.OnStartup(e);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());

            services.AddTransient<IAdminStorage, AdminStorage>();
            services.AddTransient<IDoctorStorage, DoctorStorage>();
            services.AddTransient<IScheduleStorage, ScheduleStorage>();
            services.AddTransient<IElectronicQueueStorage, ElectronicQueueStorage>();
            services.AddTransient<IAppointmentStorage, AppointmentStorage>();
            services.AddTransient<IPatientStorage, PatientStorage>();

            services.AddTransient<IPatientLogic, PatientLogic>();
            services.AddTransient<IAppointmentLogic, AppointmentLogic>();
            services.AddTransient<IElectronicQueueLogic, ElectronicQueueLogic>();
            services.AddTransient<IScheduleLogic, ScheduleLogic>();
            services.AddTransient<IAdminLogic, AdminLogic>();
            services.AddTransient<IDoctorLogic, DoctorLogic>();

            services.AddTransient<StartWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<AddDoctorWindow>();
            services.AddTransient<EditDoctorWindow>();
            services.AddTransient<EditDoctorScheduleWindow>();
            services.AddTransient<ElectronicQueueWindow>();
            services.AddTransient<DoctorWindow>();
            services.AddTransient<PatientWindow>();
            services.AddTransient<RegistrationPatientQueue>();
        }
    }
}
