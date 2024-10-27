using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataModels.Models
{
    public interface IDoctorModel: IId
    {
        string Name { get; }
        string Surname { get; }
        string? Patronymic { get; }
        string Specialization { get; }
        string Password { get; }
        int CabinetNumber { get; }
    }
}
