using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class ElectronicQueueLogic : IElectronicQueueLogic
    {
        public bool Create(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool isCompleted(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }

        public ElectronicQueueViewModel? ReadElement(ElectronicQueueSearchModel model)
        {
            throw new NotImplementedException();
        }

        public List<ElectronicQueueViewModel>? ReadList(ElectronicQueueSearchModel? model)
        {
            throw new NotImplementedException();
        }

        public bool Update(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
