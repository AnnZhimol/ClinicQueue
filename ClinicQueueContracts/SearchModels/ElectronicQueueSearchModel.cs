using ClinicQueueDataModels.Enums;

namespace ClinicQueueContracts.SearchModels
{
    public class ElectronicQueueSearchModel
    {
        public int? Id { get; set; }
        public ElectronicQueueStatus? Status { get; set; }
        public int? DoctorId { get; set; }
        public int? AdminId { get; set; }
    }
}
