using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class Patient : IPatientModel
    {
        [Required]
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string Surname { get; set; } = string.Empty;
        [DataMember]
        public string? Patronymic { get; set; } = string.Empty;
        [DataMember]
        public string? OMSNumber { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string PassportNumber { get; set; } = string.Empty;
        [Key]
        [DataMember]
        public int Id { get; set; }
        [ForeignKey("PatientId")]
        public virtual List<Appointment> Appointments { get; set; } = new();
        public static Patient? Create(PatientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Patient()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                OMSNumber = model.OMSNumber,
                PassportNumber = model.PassportNumber
            };
        }
        public static Patient Create(PatientViewModel model)
        {
            return new Patient
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                OMSNumber = model.OMSNumber,
                PassportNumber = model.PassportNumber
            };
        }
        public void Update(PatientBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Name = model.Name;
            Surname = model.Surname;
            Patronymic = model.Patronymic;
            OMSNumber = model.OMSNumber;
            PassportNumber = model.PassportNumber;
        }
        public PatientViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            OMSNumber = OMSNumber,
            PassportNumber = PassportNumber
        };
    }
}
