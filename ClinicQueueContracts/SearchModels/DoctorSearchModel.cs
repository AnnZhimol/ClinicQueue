namespace ClinicQueueContracts.SearchModels
{
    public class DoctorSearchModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Specialization { get; set; }
        public string? Password { get; set; }
    }
}
