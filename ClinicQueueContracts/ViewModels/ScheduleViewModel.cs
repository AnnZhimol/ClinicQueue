using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class ScheduleViewModel : IScheduleModel
    {
        public string DateOfWeek { get; set; } = string.Empty;
        public TimeOnly Time { get; set; }
        public int DoctorId { get; set; }
        public int AdminId { get; set; }
        public int Id { get; set; }
        public string DoctorFIO { get; set; } = string.Empty;
        public string AdminFIO { get; set; } = string.Empty;
    }
}
