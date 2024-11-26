using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IPatientLogic
    {
        List<PatientViewModel>? ReadList(PatientSearchModel? model);
        PatientViewModel? ReadElement(PatientSearchModel model);
        bool Create(PatientBindingModel model);
        bool Update(PatientBindingModel model);
        bool Delete(PatientBindingModel model);
    }
}
