using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.BindingModels
{
    public class DoctorBindingModel : IDoctorModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
    }
}
