using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.ViewModels;
using System.Windows;
using System.Windows.Threading;

namespace ClinicQueueView
{
    public partial class PatientMainWindow : Window
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly PatientViewModel _patient;
        private readonly DispatcherTimer _inactivityTimer;

        public PatientMainWindow(IPatientLogic patientLogic, PatientViewModel patient, IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic)
        {
            _patientLogic = patientLogic;
            _patient = patient;
            InitializeComponent();
            _doctorLogic = doctorLogic;
            _appointmentLogic = appointmentLogic;

            _inactivityTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _inactivityTimer.Tick += OnInactivityTimeout;
            ResetInactivityTimer();

            this.MouseMove += OnUserActivity;
            this.KeyDown += OnUserActivity;
        }

        private void OnUserActivity(object sender, EventArgs e)
        {
            ResetInactivityTimer();
        }

        private void ResetInactivityTimer()
        {
            _inactivityTimer.Stop();
            _inactivityTimer.Start();
        }

        private void OnInactivityTimeout(object? sender, EventArgs e)
        {
            _inactivityTimer.Stop();
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e) 
        {
            ReserveWindow reserveWindow = new ReserveWindow(_patientLogic, _doctorLogic, _appointmentLogic, _patient);
            reserveWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPatientQueue regWindow = new RegistrationPatientQueue(_patientLogic, _patient, _doctorLogic, _appointmentLogic);
            regWindow.Show();
            this.Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }
    }
}
