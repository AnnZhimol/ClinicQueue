namespace ClinicQueueDataModels.Models
{
    public interface IAdminModel: IId
    {
        string Name { get; }
        string Surname { get; }
        string? Patronymic { get; }
        string Password { get; }
    }
}
