using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IAppointmentStorage
    {
        List<AppointmentViewModel> GetAll();
        List<AppointmentViewModel> GetFilteredAll(AppointmentSearchModel model);
        AppointmentViewModel? GetElement(AppointmentSearchModel model);
        AppointmentViewModel? Insert(AppointmentBindingModel model);
        AppointmentViewModel? Delete(AppointmentBindingModel model);
        AppointmentViewModel? Update(AppointmentBindingModel model);
    }
}
