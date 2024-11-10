using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.BindingModels
{
    public class ElectronicQueueBindingModel : IElectronicQueueModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public ElectronicQueueStatus Status { get; set; } = ElectronicQueueStatus.Активна;
        public int DoctorId { get; set; }
        public int AdminId { get; set; }
    }
}
