using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class AppointmentViewModel : IAppointmentModel
    {
        public int? ReservationNumber { get; set; }
        public DateTime AppointmentStart { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Создан;
        public int? PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ElectronicQueueId { get; set; }
        public int Id { get; set; }
        public string PatientFIO { get; set; } = string.Empty;
        public string DoctorFIO { get; set; } = string.Empty;
        public string ElectronicQueueName { get; set; } = string.Empty;
    }
}
