using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataModels.Models
{
    public interface IScheduleModel: IId
    {
        string DateOfWeek { get; }
        TimeOnly Time {  get; }
        int DoctorId { get; }
        int AdminId { get; }
    }
}
