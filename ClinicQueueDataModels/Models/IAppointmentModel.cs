using ClinicQueueDataModels.Enums;

namespace ClinicQueueDataModels.Models
{
    public interface IAppointmentModel: IId
    {
        int? ReservationNumber { get; }
        DateTime AppointmentStart { get; }
        AppointmentStatus Status { get; }
        int? PatientId { get; }
        int DoctorId { get; }
        int ElectronicQueueId { get; }
    }
}
