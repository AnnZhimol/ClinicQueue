using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IElectronicQueueLogic
    {
        List<ElectronicQueueViewModel>? ReadList(ElectronicQueueSearchModel? model);
        ElectronicQueueViewModel? ReadElement(ElectronicQueueSearchModel model);
        bool Create(ElectronicQueueBindingModel model);
        bool Update(ElectronicQueueBindingModel model);
        bool isCompleted(ElectronicQueueBindingModel model);
    }
}
