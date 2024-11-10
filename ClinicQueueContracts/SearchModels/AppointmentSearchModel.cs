using ClinicQueueDataModels.Enums;

namespace ClinicQueueContracts.SearchModels
{
    public class AppointmentSearchModel
    {
        public int? Id { get; set; }
        public int? ReservationNumber { get; set; }
        public AppointmentStatus? Status { get; set; }
        public DateTime AppointmentStart { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? ElectronicQueueId { get; set; }
    }
}
