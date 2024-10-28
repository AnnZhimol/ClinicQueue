namespace ClinicQueueDataModels.Models
{
    public interface IPatientModel: IId
    {
        string Name { get; }
        string Surname { get; }
        string? Patronymic { get; }
        string? OMSNumber { get; }
        string PassportNumber { get; }
    }
}
