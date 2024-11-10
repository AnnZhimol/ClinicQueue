using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IAdminLogic
    {
        List<AdminViewModel>? ReadList(AdminSearchModel? model);
        AdminViewModel? ReadElement(AdminSearchModel model);
        bool Create(AdminBindingModel model);
        bool Update(AdminBindingModel model);
        bool Delete(AdminBindingModel model);
    }
}
