using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IDoctorStorage
    {
        List<DoctorViewModel> GetAll();
        List<DoctorViewModel> GetFilteredAll(DoctorSearchModel model);
        DoctorViewModel? GetElement(DoctorSearchModel model);
        DoctorViewModel? Insert(DoctorBindingModel model);
        DoctorViewModel? Delete(DoctorBindingModel model);
        DoctorViewModel? Update(DoctorBindingModel model);
    }
}
