using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Drawing.Printing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Threading;
using ClinicQueueBusinessLogic.BusinessLogic;

namespace ClinicQueueView
{
    public partial class ReserveWindow : Window
    {
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly IPatientLogic _patientLogic;
        private readonly PatientViewModel _patient;
        private string _printContent;

        private readonly DispatcherTimer _inactivityTimer;

        public ReserveWindow(IPatientLogic patientLogic,IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic, PatientViewModel patient)
        {
            _appointmentLogic = appointmentLogic;
            _doctorLogic = doctorLogic;
            _patient = patient;
            _patientLogic = patientLogic;
            InitializeComponent();
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(Specialization));
            TimeSlotComboBox.SelectionChanged += OnTimeSlotSelected;

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

        private void OnSpecializationChanged(object sender, SelectionChangedEventArgs e)
        {
            DoctorComboBox.IsEnabled = false;
            AppointmentCalendar.SelectedDate = null;
            TimeSlotComboBox.ItemsSource = null;
            TimeSlotComboBox.IsEnabled = false;
            DoctorComboBox.ItemsSource = null;

            if (SpecializationComboBox.SelectedItem != null)
            {
                var selectedSpecialization = ((Specialization)SpecializationComboBox.SelectedItem).ToString();

                var doctors = _doctorLogic.ReadList(new DoctorSearchModel
                {
                    Specialization = selectedSpecialization
                });

                DoctorComboBox.ItemsSource = doctors;
                DoctorComboBox.IsEnabled = true;
            }

            UpdateBookAppointmentButtonState();
        }

        private void OnDoctorChanged(object sender, SelectionChangedEventArgs e)
        {
            AppointmentCalendar.SelectedDate = null;
            TimeSlotComboBox.ItemsSource = null;
            TimeSlotComboBox.IsEnabled = false;

            if (DoctorComboBox.SelectedItem != null)
            {
                var selectedDoctor = (DoctorViewModel)DoctorComboBox.SelectedItem;

                AppointmentCalendar.DisplayDateStart = DateTime.Today;
                AppointmentCalendar.DisplayDateEnd = DateTime.Today.AddMonths(1);

                AppointmentCalendar.BlackoutDates.Clear();

                var appointments = _appointmentLogic.ReadList(new AppointmentSearchModel
                {
                    DoctorId = selectedDoctor.Id
                }).Where(a => a.Status == AppointmentStatus.Создан).ToList();

                var availableDates = appointments.Select(a => a.AppointmentStart.Date).Distinct().ToList();
                AppointmentCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.Today));

                for (var date = DateTime.Today; date <= AppointmentCalendar.DisplayDateEnd; date = date.AddDays(1))
                {
                    if (!availableDates.Contains(date))
                    {
                        AppointmentCalendar.BlackoutDates.Add(new CalendarDateRange(date));
                    }
                }
            }
            UpdateBookAppointmentButtonState();
        }

        private void OnDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSlotComboBox.IsEnabled = false;
            TimeSlotComboBox.ItemsSource = null;

            if (DoctorComboBox.SelectedItem != null && AppointmentCalendar.SelectedDate.HasValue)
            {
                var selectedDoctor = (DoctorViewModel)DoctorComboBox.SelectedItem;
                var selectedDate = AppointmentCalendar.SelectedDate.Value.Date;

                var appointments = _appointmentLogic.ReadList(new AppointmentSearchModel
                {
                    DoctorId = selectedDoctor.Id
                })
                .Where(a => a.AppointmentStart.Date == selectedDate && a.Status == AppointmentStatus.Создан)
                .OrderBy(a => a.AppointmentStart)
                .Select(a => a.AppointmentStart.ToLocalTime().ToString("HH:mm"))
                .ToList();

                if (appointments.Any())
                {
                    TimeSlotComboBox.ItemsSource = appointments;
                    TimeSlotComboBox.IsEnabled = true;
                }
            }
            UpdateBookAppointmentButtonState();
        }

        private void OnBookAppointmentClick(object sender, RoutedEventArgs e)
        {
            if (SpecializationComboBox.SelectedItem == null || DoctorComboBox.SelectedItem == null || AppointmentCalendar.SelectedDate == null || TimeSlotComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите специализацию, врача, дату и время приема.");
                return;
            }

            string specialization = ((Specialization)SpecializationComboBox.SelectedItem).ToString();
            var doctor = (DoctorViewModel)DoctorComboBox.SelectedItem;
            DateTime appointmentDate = AppointmentCalendar.SelectedDate.Value.Date;
            string timeSlot = TimeSlotComboBox.SelectedItem.ToString();

            DateTime localSelectedDateTime = DateTime.Parse($"{appointmentDate:yyyy-MM-dd} {timeSlot}");

            DateTime utcSelectedDateTime = localSelectedDateTime.ToUniversalTime();

            var appointment = _appointmentLogic.ReadList(new AppointmentSearchModel
            {
                DoctorId = doctor.Id
            })
            .FirstOrDefault(a => a.AppointmentStart == utcSelectedDateTime && a.Status == AppointmentStatus.Создан);

            if (appointment != null)
            {
                var random = new Random();
                int reservationNumber = random.Next(100000, 999999);
                appointment.Status = AppointmentStatus.Забронирован;
                appointment.PatientId = _patient.Id;
                appointment.ReservationNumber = reservationNumber;

                _appointmentLogic.Update(ConvertToBindingModel(appointment));

                _printContent = $"Запись на прием успешна.\nЧтобы попасть на прием, необходимо\nзарегистрироваться в электронной очереди\n за 10 минут до начала приема. \nВ противном случае, прием будет отменен.\nДля регистрации потребуется номер брони.\n\n" +
                        $"Номер брони: {reservationNumber}\n" +
                        $"Специализация: {specialization}\n" +
                        $"Врач: {doctor.FullName}\n" +
                        $"Дата: {appointmentDate:dd.MM.yyyy}\n" +
                        $"Время: {timeSlot}";

                MessageBox.Show(_printContent);

                PrintReceipt();
                _inactivityTimer.Stop();
                PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
                patientWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выбранное время уже занято или недоступно.");
            }
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _inactivityTimer.Stop();
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }

        private void OnTimeSlotSelected(object sender, SelectionChangedEventArgs e)
        {
            UpdateBookAppointmentButtonState();
        }

        private void UpdateBookAppointmentButtonState()
        {
            ReserveButton.IsEnabled = SpecializationComboBox.SelectedItem != null &&
                                              DoctorComboBox.SelectedItem != null &&
                                              AppointmentCalendar.SelectedDate != null &&
                                              TimeSlotComboBox.SelectedItem != null;
        }
    }
}
