using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Windows;
using System.Windows.Input;

namespace ClinicQueueView
{
    public partial class ElectronicQueueWindow : Window
    {
        private readonly DoctorViewModel _doctor;
        private readonly AdminViewModel _admin;
        private readonly IElectronicQueueLogic _queueLogic;
        private readonly IScheduleLogic _scheduleLogic;
        private readonly IAppointmentLogic _appointmentLogic;

        public ElectronicQueueWindow(DoctorViewModel doctor, AdminViewModel admin, IElectronicQueueLogic queueLogic, IScheduleLogic scheduleLogic, IAppointmentLogic appointmentLogic)
        {

            _queueLogic = queueLogic;
            _scheduleLogic = scheduleLogic;
            _appointmentLogic = appointmentLogic;
            _admin = admin;
            _doctor = doctor;
            InitializeComponent();

            LoadQueues();
        }

        private void LoadQueues()
        {
            if (_doctor == null)
            {
                MessageBox.Show("Doctor object is null.");
                return;
            }

            if (_queueLogic == null)
            {
                MessageBox.Show("Queue logic is not initialized.");
                return;
            }
            QueueDataGrid.ItemsSource = _queueLogic.ReadList(new ElectronicQueueSearchModel { DoctorId = _doctor.Id });
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateQueueButton_Click(object sender, RoutedEventArgs e)
        {
            string queueName = QueueNameTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(queueName) || startDate == null || endDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (endDate <= startDate)
            {
                MessageBox.Show("Дата окончания должна быть позже даты начала.");
                return;
            }

            if (endDate < DateTime.Now || startDate < DateTime.Now)
            {
                MessageBox.Show("Даты должны быть больше сегодняшнего дня.");
                return;
            }

            var activeQueue = _queueLogic.ReadList(new ElectronicQueueSearchModel 
            { 
                DoctorId = _doctor.Id
            })?.FirstOrDefault();

            if (activeQueue != null)
            {
                var canceledAppointment = _appointmentLogic.ReadList(new AppointmentSearchModel
                {
                    ElectronicQueueId = activeQueue.Id,
                    AppointmentStart = DateTime.Now
                });

                foreach (var appointment in canceledAppointment)
                {
                    if(appointment.Status == AppointmentStatus.Создан)
                        appointment.Status = AppointmentStatus.Отменен;
                    _appointmentLogic.Update(ConvertToBindingModel(appointment));
                }
                activeQueue.Status = ElectronicQueueStatus.Завершена;

                var activeQueueBindingModel = ConvertToBindingModel(activeQueue);
                if (activeQueueBindingModel != null)
                {
                    _queueLogic.Update(activeQueueBindingModel);
                }
            }

            var newQueue = new ElectronicQueueBindingModel
            {
                Name = queueName,
                StartDate = startDate.Value.ToUniversalTime(),
                EndDate = endDate.Value.ToUniversalTime(),
                Status = ElectronicQueueStatus.Активна,
                DoctorId = _doctor.Id,
                AdminId = _admin.Id
            };

            activeQueue = _queueLogic.Create(newQueue);

            CreateAppointments(activeQueue);

            MessageBox.Show("Новая очередь создана и активирована.");
            LoadQueues();
            this.Close();
        }

        private void CreateAppointments(ElectronicQueueViewModel? queue)
        {
            var dayOfWeekMapping = new Dictionary<string, string> {
                { "Monday", "Понедельник" },
                { "Tuesday", "Вторник" },
                { "Wednesday", "Среда" },
                { "Thursday", "Четверг" },
                { "Friday", "Пятница" },
                { "Saturday", "Суббота" },
                { "Sunday", "Воскресенье" }
            };

            var allSchedules = _scheduleLogic.ReadList(new ScheduleSearchModel { DoctorId = _doctor.Id });

            for (var date = queue.StartDate; date <= queue.EndDate; date = date.AddDays(1))
            {
                var russianDayOfWeek = dayOfWeekMapping[date.DayOfWeek.ToString()];

                var schedules = allSchedules
                    .Where(s => s.DateOfWeek == russianDayOfWeek)
                    .ToList();

                foreach (var schedule in schedules)
                {
                    var appointment = new AppointmentBindingModel
                    {
                        DoctorId = _doctor.Id,
                        ElectronicQueueId = queue.Id,
                        AppointmentStart = new DateTime(date.Year, date.Month, date.Day, schedule.Time.Hour, schedule.Time.Minute, 0).ToUniversalTime(),
                        Status = AppointmentStatus.Создан
                    };

                    _appointmentLogic.Create(appointment);
                }
            }
        }
        private void QueueListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (QueueDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите очередь для выполнения действия.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopQueue_Click(object sender, RoutedEventArgs e)
        {
            if (QueueDataGrid.SelectedItem is ElectronicQueueViewModel selectedQueue)
            {
                if (selectedQueue.Status == ElectronicQueueStatus.Завершена)
                {
                    MessageBox.Show("Эта очередь уже завершена.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите завершить очередь {selectedQueue.Name}?", "Подтверждение завершения", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (selectedQueue != null)
                    {
                        var canceledAppointment = _appointmentLogic.ReadList(new AppointmentSearchModel
                        {
                            ElectronicQueueId = selectedQueue.Id,
                            AppointmentStart = DateTime.Now
                        });

                        foreach (var appointment in canceledAppointment)
                        {
                            if (appointment.Status == AppointmentStatus.Создан)
                                appointment.Status = AppointmentStatus.Отменен;
                            _appointmentLogic.Update(ConvertToBindingModel(appointment));
                        }
                        selectedQueue.Status = ElectronicQueueStatus.Завершена;

                        var activeQueueBindingModel = ConvertToBindingModel(selectedQueue);
                        if (activeQueueBindingModel != null)
                        {
                            _queueLogic.Update(activeQueueBindingModel);
                        }
                    }

                    MessageBox.Show("Очередь завершена.");
                    LoadQueues();
                }
            }
        }

        public static ElectronicQueueBindingModel ConvertToBindingModel(ElectronicQueueViewModel viewModel)
        {
            if (viewModel == null)
                return null;

            return new ElectronicQueueBindingModel
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                Status = viewModel.Status,
                DoctorId = viewModel.DoctorId,
                AdminId = viewModel.AdminId
            };
        }

        public static AppointmentBindingModel ConvertToBindingModel(AppointmentViewModel viewModel)
        {
            if (viewModel == null)
                return null;

            return new AppointmentBindingModel
            {
                Id = viewModel.Id,
                AppointmentStart = viewModel.AppointmentStart,
                Status = viewModel.Status,
                DoctorId = viewModel.DoctorId,
                ElectronicQueueId = viewModel.ElectronicQueueId
            };
        }
    }
}
