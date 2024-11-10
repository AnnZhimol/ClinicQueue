using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IAdminStorage
    {
        List<AdminViewModel> GetAll();
        List<AdminViewModel> GetFilteredAll(AdminSearchModel model);
        AdminViewModel? GetElement(AdminSearchModel model);
        AdminViewModel? Insert(AdminBindingModel model);
        AdminViewModel? Delete(AdminBindingModel model);
        AdminViewModel? Update(AdminBindingModel model);
    }
}
