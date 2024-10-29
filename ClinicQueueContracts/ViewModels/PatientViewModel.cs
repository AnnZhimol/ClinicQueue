using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class PatientViewModel : IPatientModel
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string? Patronymic { get; set; } = string.Empty;

        public string? OMSNumber { get; set; } = string.Empty;

        public string PassportNumber { get; set; } = string.Empty;

        public int Id { get; set; }
    }
}
