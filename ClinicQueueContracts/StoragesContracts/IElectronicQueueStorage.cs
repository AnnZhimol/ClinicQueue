using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;

namespace ClinicQueueContracts.StoragesContracts
{
    public interface IElectronicQueueStorage
    {
        List<ElectronicQueueViewModel> GetAll();
        List<ElectronicQueueViewModel> GetFilteredAll(ElectronicQueueSearchModel model);
        ElectronicQueueViewModel? GetElement(ElectronicQueueSearchModel model);
        ElectronicQueueViewModel? Insert(ElectronicQueueBindingModel model);
        ElectronicQueueViewModel? Delete(ElectronicQueueBindingModel model);
        ElectronicQueueViewModel? Update(ElectronicQueueBindingModel model);
    }
}
