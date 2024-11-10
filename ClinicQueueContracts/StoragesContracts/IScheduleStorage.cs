using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IScheduleStorage
    {
        List<ScheduleViewModel> GetAll();
        List<ScheduleViewModel> GetFilteredAll(ScheduleSearchModel model);
        ScheduleViewModel? GetElement(ScheduleSearchModel model);
        ScheduleViewModel? Insert(ScheduleBindingModel model);
        ScheduleViewModel? Delete(ScheduleBindingModel model);
        ScheduleViewModel? Update(ScheduleBindingModel model);
    }
}
