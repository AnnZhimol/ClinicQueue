using ClinicQueueDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataModels.Models
{
    public interface IElectronicQueueModel : IId
    {
        string Name { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        ElectronicQueueStatus Status { get; }
        int DoctorId { get; }
        int AdminId { get; }
    }
}
