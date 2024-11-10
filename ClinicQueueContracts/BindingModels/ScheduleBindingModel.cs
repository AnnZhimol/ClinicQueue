using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.BindingModels
{
    public class ScheduleBindingModel : IScheduleModel
    {
        public int Id { get; set; }
        public string DateOfWeek { get; set; } = string.Empty;
        public TimeOnly Time { get; set; }
        public int DoctorId { get; set; }
        public int AdminId { get; set; }
    }
}
