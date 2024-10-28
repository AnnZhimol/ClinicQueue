namespace ClinicQueueContracts.SearchModels
{
    public class PatientSearchModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? OMSNumber { get; set; }
        public string? PassportNumber { get; set; }
    }
}
