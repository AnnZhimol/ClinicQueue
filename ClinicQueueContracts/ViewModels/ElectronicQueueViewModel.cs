using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class ElectronicQueueViewModel : IElectronicQueueModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public ElectronicQueueStatus Status { get; set; } = ElectronicQueueStatus.Активна;
        public int DoctorId { get; set; }
        public int AdminId { get; set; }
        public int Id { get; set; }
        public string DoctorFIO { get; set; } = string.Empty;
        public string AdminFIO { get; set; } = string.Empty;
    }
}
