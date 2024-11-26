using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ClinicQueueView
{
    public partial class RegistrationPatientQueue : Window
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly PatientViewModel _patient;
        private string _printContent;

        private readonly DispatcherTimer _inactivityTimer;

        public RegistrationPatientQueue(IPatientLogic patientLogic, PatientViewModel patient, IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic)
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

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string number = NumberInputTextBox.Text;
            if (string.IsNullOrEmpty(number))
            {
                MessageBox.Show("Пожалуйста, введите номер брони. Его можно получить при записи к врачу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var appointment = _appointmentLogic.ReadElement(new AppointmentSearchModel
            {
                ReservationNumber = Int32.Parse(number),
                Status = AppointmentStatus.Забронирован,
                PatientId =_patient.Id
            });

            if (appointment != null)
            {
                if (appointment.AppointmentStart.ToLocalTime() >= DateTime.Now && appointment.AppointmentStart.ToLocalTime() <= DateTime.Now.AddMinutes(10))
                {
                    appointment.Status = AppointmentStatus.Отменен;
                    _appointmentLogic.Update(ConvertToBindingModel(appointment));
                    MessageBox.Show("Регистрация провалилась. До начала приема осталось менее 10 минут.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (appointment.AppointmentStart.ToLocalTime() <= DateTime.Now)
                {
                    appointment.Status = AppointmentStatus.Отменен;
                    _appointmentLogic.Update(ConvertToBindingModel(appointment));
                    MessageBox.Show("Регистрация провалилась. Прием окончен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (appointment.AppointmentStart.Date != DateTime.Today)
                {
                    MessageBox.Show("На прием можно зарегистрироваться не раньше дня приема.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                appointment.Status = AppointmentStatus.Ожидание;

                _appointmentLogic.Update(ConvertToBindingModel(appointment));
                var doctor = _doctorLogic.ReadElement(new DoctorSearchModel { Id = appointment.DoctorId });

                _printContent = $"Регистрация на прием успешна.\nЧтобы попасть на прием, пройдите\nв указанный на талоне кабинет\nи ждите приглашения.\n\n" +
                        $"Специализация: {doctor?.Specialization}\n" +
                        $"Врач: {appointment.DoctorFIO}\n" +
                        $"Кабинет: {doctor?.CabinetNumber}\n" +
                        $"Дата: {appointment.AppointmentStart.Date:dd.MM.yyyy}\n" +
                        $"Время: {appointment.AppointmentStart.ToLocalTime().ToString("HH:mm")}";

                MessageBox.Show(_printContent);
                PrintReceipt();
                _inactivityTimer.Stop();
                PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
                patientWindow.Show();
                this.Close();
            } 
            else MessageBox.Show("Прием с данным номером брони не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _inactivityTimer.Stop();
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }

        public AppointmentBindingModel ConvertToBindingModel(AppointmentViewModel appointmentViewModel)
        {
            if (appointmentViewModel == null)
            {
                throw new ArgumentNullException(nameof(appointmentViewModel));
            }

            return new AppointmentBindingModel
            {
                Id = appointmentViewModel.Id,
                DoctorId = appointmentViewModel.DoctorId,
                AppointmentStart = appointmentViewModel.AppointmentStart,
                Status = appointmentViewModel.Status,
                PatientId = appointmentViewModel.PatientId,
                ReservationNumber = appointmentViewModel.ReservationNumber,
                ElectronicQueueId = appointmentViewModel.ElectronicQueueId
            };
        }

        private void PrintReceipt()
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка печати: {ex.Message}");
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(
            _printContent,
            new System.Drawing.Font("Arial", 12),
            System.Drawing.Brushes.Black,
            new RectangleF(10, 10, e.PageBounds.Width - 20, e.PageBounds.Height - 20)
        );
        }
    }
}
