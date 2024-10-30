using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class ElectronicQueueStorage : IElectronicQueueStorage
    {
        public ElectronicQueueViewModel? Delete(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }

        public List<ElectronicQueueViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public ElectronicQueueViewModel? GetElement(ElectronicQueueSearchModel model)
        {
            throw new NotImplementedException();
        }

        public List<ElectronicQueueViewModel> GetFilteredAll(ElectronicQueueSearchModel model)
        {
            throw new NotImplementedException();
        }

        public ElectronicQueueViewModel? Insert(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }

        public ElectronicQueueViewModel? Update(ElectronicQueueBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
