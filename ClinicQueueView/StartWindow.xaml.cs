using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueDataBaseImplement.Models;
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
using System.Windows.Shapes;

namespace ClinicQueueView
{
    public partial class StartWindow : Window
    {
        private readonly IAdminLogic _adminLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IScheduleLogic _scheduleLogic;
        private readonly IElectronicQueueLogic _electronicQueueLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly IPatientLogic _patientLogic;
        public StartWindow(IAdminLogic adminLogic, IDoctorLogic doctorLogic, IScheduleLogic scheduleLogic, IElectronicQueueLogic electronicQueueLogic, IAppointmentLogic appointmentLogic, IPatientLogic patientLogic)
        {
            _adminLogic = adminLogic;
            _doctorLogic = doctorLogic;
            _scheduleLogic = scheduleLogic;
            _appointmentLogic = appointmentLogic;
            _electronicQueueLogic = electronicQueueLogic;
            _patientLogic = patientLogic;
            InitializeComponent();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Запуск окна для авторизации сотрудника.", "Вход", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow mainWindow = new MainWindow(_patientLogic, _adminLogic, _doctorLogic, _scheduleLogic, _appointmentLogic, _electronicQueueLogic);
            mainWindow.Show();
            Close();
        }

        private void PatientButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
