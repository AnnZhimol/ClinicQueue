using ClinicQueueContracts.BusinessLogicContracts;
using System.Windows;

namespace ClinicQueueView
{
    public partial class PatientWindow : Window
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        public PatientWindow(IPatientLogic patientLogic, IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic)
        {
            _patientLogic = patientLogic;
            InitializeComponent();
            _doctorLogic = doctorLogic;
            _appointmentLogic = appointmentLogic;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationPatient authorizationPatient = new AuthorizationPatient(_patientLogic, _appointmentLogic, _doctorLogic);
            authorizationPatient.Show();
            Close();
        }
    }
}
