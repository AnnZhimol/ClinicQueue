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
    public interface IDoctorLogic
    {
        List<DoctorViewModel>? ReadList(DoctorSearchModel? model);
        DoctorViewModel? ReadElement(DoctorSearchModel model);
        bool Create(DoctorBindingModel model);
        bool Update(DoctorBindingModel model);
        bool Delete(DoctorBindingModel model);
    }
}
