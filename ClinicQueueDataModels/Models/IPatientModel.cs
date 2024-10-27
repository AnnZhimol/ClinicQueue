using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataModels.Models
{
    public interface IPatientModel: IId
    {
        string Name { get; }
        string Surname { get; }
        string? Patronymic { get; }
        string? OMSNumber { get; }
        string PassportNumber { get; }
    }
}
