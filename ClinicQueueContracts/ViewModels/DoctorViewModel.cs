using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class DoctorViewModel : IDoctorModel
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
        public int Id { get; set; }
    }
}
