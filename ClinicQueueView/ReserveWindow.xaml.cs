using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Windows;
using System.Windows.Controls;

namespace ClinicQueueView
{
    public partial class ReserveWindow : Window
    {
        private readonly IDoctorLogic _doctorLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly PatientViewModel _patient;

        public ReserveWindow(IDoctorLogic doctorLogic, IAppointmentLogic appointmentLogic, PatientViewModel patient)
        {
            _appointmentLogic = appointmentLogic;
            _doctorLogic = doctorLogic;
            _patient = patient;
            InitializeComponent();
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(Specialization));
            TimeSlotComboBox.SelectionChanged += OnTimeSlotSelected;
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
                if (selectedSpecialization == "Нет")
                {
                    MessageBox.Show("Пожалуйста, выберите специализацию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
            var timeSlot = TimeSlotComboBox.SelectedItem.ToString();

            // Создаем объект DateTime с учетом выбранной даты и времени
            DateTime selectedDateTime = DateTime.Parse($"{appointmentDate:yyyy-MM-dd} {timeSlot}");

            // Пытаемся найти прием без преобразования в строку
            var appointment = _appointmentLogic.ReadList(new AppointmentSearchModel
            {
                DoctorId = doctor.Id
            })
            .FirstOrDefault(a => a.AppointmentStart == selectedDateTime && a.Status == AppointmentStatus.Создан);

            if (appointment != null)
            {
                var random = new Random();
                int reservationNumber = random.Next(100000, 999999);
                appointment.Status = AppointmentStatus.Забронирован;
                appointment.PatientId = _patient.Id;
                appointment.ReservationNumber = reservationNumber;

                _appointmentLogic.Update(ConvertToBindingModel(appointment));

                MessageBox.Show($"Запись на прием успешна:\n\nСпециализация: {specialization}\nВрач: {doctor.FullName}\nКабинет: {doctor.CabinetNumber}\nДата: {appointmentDate:dd.MM.yyyy}\nВремя: {timeSlot}");
                Close();
            }
            else
            {
                MessageBox.Show("Выбранное время уже занято или недоступно.");
            }
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
