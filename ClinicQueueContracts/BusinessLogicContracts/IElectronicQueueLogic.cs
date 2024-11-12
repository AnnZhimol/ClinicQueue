using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IElectronicQueueLogic
    {
        List<ElectronicQueueViewModel>? ReadList(ElectronicQueueSearchModel? model);
        ElectronicQueueViewModel? ReadElement(ElectronicQueueSearchModel model);
        ElectronicQueueViewModel? Create(ElectronicQueueBindingModel model);
        bool Update(ElectronicQueueBindingModel model);
        bool isCompleted(ElectronicQueueBindingModel model);
    }
}
