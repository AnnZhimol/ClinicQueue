namespace ClinicQueueDataModels.Models
{
    public interface IDoctorModel: IId
    {
        string Name { get; }
        string Surname { get; }
        string? Patronymic { get; }
        string Specialization { get; }
        string Password { get; }
        int CabinetNumber { get; }
    }
}
