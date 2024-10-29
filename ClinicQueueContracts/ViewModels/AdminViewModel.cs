using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.ViewModels
{
    public class AdminViewModel : IAdminModel
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}
