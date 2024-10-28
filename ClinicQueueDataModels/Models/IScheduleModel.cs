namespace ClinicQueueDataModels.Models
{
    public interface IScheduleModel: IId
    {
        string DateOfWeek { get; }
        TimeOnly Time {  get; }
        int DoctorId { get; }
        int AdminId { get; }
    }
}
