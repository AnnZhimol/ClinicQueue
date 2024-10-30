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
    public class PatientStorage : IPatientStorage
    {
        public PatientViewModel? Delete(PatientBindingModel model)
        {
            throw new NotImplementedException();
        }

        public List<PatientViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public PatientViewModel? GetElement(PatientSearchModel model)
        {
            throw new NotImplementedException();
        }

        public List<PatientViewModel> GetFilteredAll(PatientSearchModel model)
        {
            throw new NotImplementedException();
        }

        public PatientViewModel? Insert(PatientBindingModel model)
        {
            throw new NotImplementedException();
        }

        public PatientViewModel? Update(PatientBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
