﻿using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueContracts.BusinessLogicContracts
{
    public interface IAppointmentLogic
    {
        List<AppointmentViewModel>? ReadList(AppointmentSearchModel? model);
        AppointmentViewModel? ReadElement(AppointmentSearchModel model);
        bool Create(AppointmentBindingModel model);
        bool Update(AppointmentBindingModel model);
        bool Delete(AppointmentBindingModel model);
        bool CancelAppointment(AppointmentBindingModel model);
        bool ReserveAppointment(AppointmentBindingModel model);
        bool inWaiting(AppointmentBindingModel model);
        bool inProcessing(AppointmentBindingModel model);
        bool isCompleted(AppointmentBindingModel model);
    }
}
