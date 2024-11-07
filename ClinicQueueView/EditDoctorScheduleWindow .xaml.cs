using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Models;
using System.Globalization;
using System.Windows;

namespace ClinicQueueView
{
    public partial class EditDoctorScheduleWindow : Window
    {
        private readonly IScheduleLogic _scheduleLogic;
        private readonly IDoctorLogic _doctorLogic;
        private readonly DoctorViewModel _doctor;
        private readonly AdminViewModel _admin;

        public EditDoctorScheduleWindow(IScheduleLogic scheduleLogic, IDoctorLogic doctorLogic, DoctorViewModel doctor, AdminViewModel admin)
        {
            InitializeComponent();
            _scheduleLogic = scheduleLogic;
            _doctorLogic = doctorLogic;
            _doctor = doctor;
            _admin = admin;
            Loaded += EditDoctorScheduleWindow_Loaded;
            InitializeDaysOfWeek();
        }

        private void EditDoctorScheduleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorEditSchedule.Text = $"Редактирование расписания врача\n{_doctor.Surname} {_doctor.Name} {_doctor.Patronymic}";
        }

        private void InitializeDaysOfWeek()
        {
            var daysOfWeek = new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

            var daysWithIntervals = daysOfWeek.Select(day => new
            {
                Day = day,
                Intervals = GetTimeIntervals(day).Select(time => new TimeInterval { Time = time, IsSelected = false }).ToList()
            }).ToList();

            var existingSchedules = _scheduleLogic.ReadList(new ScheduleSearchModel
            {
                DoctorId = _doctor.Id
            });

            if (existingSchedules != null && existingSchedules.Any())
            {
                foreach (var dayItem in daysWithIntervals)
                {
                    foreach (var interval in dayItem.Intervals)
                    {
                        var existingSchedule = existingSchedules.FirstOrDefault(s => s.DateOfWeek == dayItem.Day && s.Time.ToString("HH:mm") == interval.Time);
                        if (existingSchedule != null)
                        {
                            interval.IsSelected = true;
                        }
                    }
                }
            }

            DaysOfWeekControl.ItemsSource = daysWithIntervals;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private List<string> GetTimeIntervals(string day)
        {
            var intervals = new List<string>();

            var isWeekend = day == "Суббота" || day == "Воскресенье";
            var startTime = isWeekend ? new TimeOnly(10, 0) : new TimeOnly(8, 0);
            var endTime = isWeekend ? new TimeOnly(15, 0) : new TimeOnly(18, 0);

            for (var time = startTime; time < endTime; time = time.AddMinutes(30))
            {
                intervals.Add(time.ToString("HH:mm", CultureInfo.InvariantCulture));
            }

            return intervals;
        }

        private void SaveScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_doctor == null)
            {
                MessageBox.Show("Пожалуйста, выберите врача.");
                return;
            }

            var selectedSchedules = new List<ScheduleBindingModel>();

            foreach (var dayItem in DaysOfWeekControl.Items)
            {
                dynamic dayContext = dayItem;
                var dayOfWeek = dayContext.Day;

                foreach (var interval in dayContext.Intervals)
                {
                    if (interval.IsSelected)
                    {
                        if (TimeOnly.TryParse(interval.Time, out TimeOnly time))
                        {
                            selectedSchedules.Add(new ScheduleBindingModel
                            {
                                DoctorId = _doctor.Id,
                                DateOfWeek = dayOfWeek,
                                Time = time,
                                AdminId = _admin.Id
                            });
                        }
                    }
                }
            }

            var existingSchedules = _scheduleLogic.ReadList(new ScheduleSearchModel
            {
                DoctorId = _doctor.Id
            });

            if (existingSchedules != null)
            {
                foreach (var schedule in existingSchedules)
                {
                    _scheduleLogic.Delete(ToBindingModel(schedule));
                }
            }

            foreach (var schedule in selectedSchedules)
            {
                _scheduleLogic.Create(schedule);
            }

            MessageBox.Show("Расписание успешно сохранено.");
            this.Close();
        }


        private ScheduleBindingModel ToBindingModel(ScheduleViewModel viewModel)
        {
            return new ScheduleBindingModel
            {
                Id = viewModel.Id,
                DateOfWeek = viewModel.DateOfWeek,
                Time = viewModel.Time,
                DoctorId = viewModel.DoctorId,
                AdminId = viewModel.AdminId
            };
        }
    }
}
