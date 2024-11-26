using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IDoctorLogic
    {
        List<DoctorViewModel>? ReadList(DoctorSearchModel? model);
        DoctorViewModel? ReadElement(DoctorSearchModel model);
        bool Create(DoctorBindingModel model);
        bool Update(DoctorBindingModel model);
        bool Delete(DoctorBindingModel model);
    }
}
