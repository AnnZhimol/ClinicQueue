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
    public class AppointmentLogic : IAppointmentLogic
    {
        public bool CancelAppointment(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool Create(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool inProcessing(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool inWaiting(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool isCompleted(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public AppointmentViewModel? ReadElement(AppointmentSearchModel model)
        {
            throw new NotImplementedException();
        }

        public List<AppointmentViewModel>? ReadList(AppointmentSearchModel? model)
        {
            throw new NotImplementedException();
        }

        public bool ReserveAppointment(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(AppointmentBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
