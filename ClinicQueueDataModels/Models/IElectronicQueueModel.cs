using ClinicQueueDataModels.Enums;

namespace ClinicQueueDataModels.Models
{
    public interface IElectronicQueueModel : IId
    {
        string Name { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        ElectronicQueueStatus Status { get; }
        int DoctorId { get; }
        int AdminId { get; }
    }
}
