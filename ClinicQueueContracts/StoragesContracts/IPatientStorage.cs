using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IPatientStorage
    {
        List<PatientViewModel> GetAll();
        List<PatientViewModel> GetFilteredAll(PatientSearchModel model);
        PatientViewModel? GetElement(PatientSearchModel model);
        PatientViewModel? Insert(PatientBindingModel model);
        PatientViewModel? Delete(PatientBindingModel model);
        PatientViewModel? Update(PatientBindingModel model);
    }
}
