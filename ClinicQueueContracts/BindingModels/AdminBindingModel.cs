using ClinicQueueDataModels.Models;

namespace ClinicQueueContracts.BindingModels
{
    public class AdminBindingModel : IAdminModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Patronymic { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
