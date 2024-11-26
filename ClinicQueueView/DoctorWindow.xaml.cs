using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClinicQueueView
{
    public partial class DoctorWindow : Window
    {
        private readonly DoctorViewModel _doctor;
        private readonly IAdminLogic _adminLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly IScheduleLogic _scheduleLogic;
        private readonly IElectronicQueueLogic _electronicQueueLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly IPatientLogic _patientLogic;
        private List<AppointmentViewModel> _allAppointments;

        public DoctorWindow(IAdminLogic adminLogic, IDoctorLogic doctorLogic, IScheduleLogic scheduleLogic, IElectronicQueueLogic electronicQueueLogic, IAppointmentLogic appointmentLogic, IPatientLogic patientLogic, DoctorViewModel doctor)
        {
            _adminLogic = adminLogic;
            _doctorLogic = doctorLogic;
            _scheduleLogic = scheduleLogic;
            _patientLogic = patientLogic;
            _appointmentLogic = appointmentLogic;
            _patientLogic = patientLogic;
            _doctor = doctor;
            InitializeComponent();
            Loaded += DoctorWindow_Loaded;
            StatusFilterComboBox.ItemsSource = Enum.GetValues(typeof(AppointmentStatus));
            LoadPatients();
        }

        private void PatientButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DoctorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WelcomeTextBox.Text = $"Добро пожаловать,\n{_doctor.Surname} {_doctor.Name} {_doctor.Patronymic}";
        }

        private void LoadPatients()
        {
            PatientsListView.ItemsSource = _allAppointments = _appointmentLogic.ReadList(new AppointmentSearchModel
            {
                DoctorId = _doctor.Id,
            }).Where(x => !string.IsNullOrEmpty(x.PatientFIO)).Select(appointment =>
            {
                appointment.AppointmentStart = appointment.AppointmentStart.ToLocalTime();
                return appointment;
            }).ToList();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow(_patientLogic, _adminLogic, _doctorLogic, _scheduleLogic, _appointmentLogic, _electronicQueueLogic);
                mainWindow.Show();
                this.Close();
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

        private void StartAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem is AppointmentViewModel selectedPatient)
            {
                if (selectedPatient.Status != AppointmentStatus.Ожидание)
                {
                    MessageBox.Show("Пожалуйста, выберите пациента со статусом 'Ожидание'.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите начать прием c {selectedPatient.PatientFIO}?", "Подтверждение начала", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _appointmentLogic.inProcessing(ConvertToBindingModel(selectedPatient));
                    var result2 = MessageBox.Show($"Прием с пациентом {selectedPatient.PatientFIO}начат. Чтобы завершить прием, нажмите ОК", "Прием", MessageBoxButton.OK, MessageBoxImage.Question);
                    if (result2 == MessageBoxResult.OK)
                    {
                        _appointmentLogic.isCompleted(ConvertToBindingModel(selectedPatient));
                    }
                    LoadPatients();
                }
            }
        }

        private void StopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem is AppointmentViewModel selectedPatient)
            {
                if (selectedPatient.Status != AppointmentStatus.Ожидание)
                {
                    MessageBox.Show("Пожалуйста, выберите пациента со статусом 'Ожидание'.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите отменить прием c {selectedPatient.PatientFIO}?", "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _appointmentLogic.inProcessing(ConvertToBindingModel(selectedPatient));
                    var result2 = MessageBox.Show($"Прием с пациентом {selectedPatient.PatientFIO} отменен.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                    _appointmentLogic.CancelAppointment(ConvertToBindingModel(selectedPatient));
                    LoadPatients();
                }
            }
        }

        private void UpdateDoctorsList()
        {
            var filteredPatients = _allAppointments;

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                var searchFIO = SearchTextBox.Text.ToLower();
                filteredPatients = filteredPatients?
                    .Where(d => d.PatientFIO.ToLower().Contains(searchFIO))
                    .ToList();
            }

            if (StatusFilterComboBox != null && StatusFilterComboBox.SelectedItem is AppointmentStatus selectedStatus)
            {
                if (((AppointmentStatus)StatusFilterComboBox.SelectedItem).ToString() != "Нет")
                {
                    filteredPatients = filteredPatients?
                        .Where(d => d.Status == selectedStatus)
                        .ToList();
                }
                else
                {
                    filteredPatients = filteredPatients?
                        .Where(d => d.Status != null)
                        .ToList();
                }
            }

            if (PatientsListView != null)
            {
                PatientsListView.ItemsSource = filteredPatients;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDoctorsList();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDoctorsList();
        }

        private void PatientsListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PatientsListView.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите пациента для выполнения действия.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
