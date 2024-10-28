using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.BindingModels
{
    public class AppointmentBindingModel : IAppointmentModel
    {
        public int Id { get; set; }
        public int? ReservationNumber { get; set; }
        public DateTime AppointmentStart { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Created;
        public int? PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ElectronicQueueId { get; set; }
        //public string PatientFIO { get; set; } = string.Empty;
        //public string DoctorFIO { get; set; } = string.Empty;
        //public int AppointmentCabinet { get; set; }
    }
}
