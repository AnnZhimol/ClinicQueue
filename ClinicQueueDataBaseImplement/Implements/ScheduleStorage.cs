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
    public class ScheduleStorage : IScheduleStorage
    {
        public ScheduleViewModel? Delete(ScheduleBindingModel model)
        {
            throw new NotImplementedException();
        }

        public List<ScheduleViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public ScheduleViewModel? GetElement(ScheduleSearchModel model)
        {
            throw new NotImplementedException();
        }

        public List<ScheduleViewModel> GetFilteredAll(ScheduleSearchModel model)
        {
            throw new NotImplementedException();
        }

        public ScheduleViewModel? Insert(ScheduleBindingModel model)
        {
            throw new NotImplementedException();
        }

        public ScheduleViewModel? Update(ScheduleBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
