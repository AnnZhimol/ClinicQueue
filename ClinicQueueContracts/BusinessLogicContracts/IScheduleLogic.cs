using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IScheduleLogic
    {
        List<ScheduleViewModel>? ReadList(ScheduleSearchModel? model);
        ScheduleViewModel? ReadElement(ScheduleSearchModel model);
        bool Create(ScheduleBindingModel model);
        bool Update(ScheduleBindingModel model);
        bool Delete(ScheduleBindingModel model);
    }
}
