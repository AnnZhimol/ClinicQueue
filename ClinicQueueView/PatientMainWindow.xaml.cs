using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.ViewModels;
using System.Windows;

namespace ClinicQueueView
{
    public partial class PatientMainWindow : Window
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly PatientViewModel _patient;

        public PatientMainWindow(IPatientLogic patientLogic, PatientViewModel patient, IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic)
        {
            _patientLogic = patientLogic;
            _patient = patient;
            InitializeComponent();
            _doctorLogic = doctorLogic;
            _appointmentLogic = appointmentLogic;
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e) 
        {
            ReserveWindow reserveWindow = new ReserveWindow(_doctorLogic, _appointmentLogic, _patient);
            reserveWindow.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }
    }
}
