using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class ElectronicQueue : IElectronicQueueModel
    {
        [Required]
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required]
        [DataMember]
        public DateTime EndDate { get; set; }
        [Required]
        [DataMember]
        public ElectronicQueueStatus Status { get; set; } = ElectronicQueueStatus.Активна;
        [Required]
        [DataMember]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        [Required]
        [DataMember]
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        [Key]
        [DataMember]
        public int Id { get; private set; }
        [ForeignKey("ElectronicQueueId")]
        public virtual List<Appointment> Appointments { get; set; } = new();
        public static ElectronicQueue? Create(ElectronicQueueBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }

            return new ElectronicQueue()
            {
                Id = model.Id,
                AdminId = model.AdminId,
                DoctorId = model.DoctorId,
                Status = model.Status,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                Name = model.Name
            };
        }

        public void Update(ElectronicQueueBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Status = model.Status;
            AdminId = model.AdminId;
        }

        public ElectronicQueueViewModel GetViewModel
        {
            get
            {
                using var context = new ClinicQueueDataBase();
                var admin = context.Admins.FirstOrDefault(x => x.Id == AdminId);
                var doctor = context.Doctors.FirstOrDefault(x => x.Id == DoctorId);
                return new ElectronicQueueViewModel
                {
                    Id = Id,
                    AdminId = AdminId,
                    DoctorId = DoctorId,
                    Status = Status,
                    StartDate= StartDate,
                    EndDate= EndDate,
                    Name = Name,
                    AdminFIO = admin != null
                        ? $"{admin.Surname} {admin.Name} {admin.Patronymic}"
                        : string.Empty,
                    DoctorFIO = doctor != null
                        ? $"{doctor.Surname} {doctor.Name} {doctor.Patronymic}"
                        : string.Empty,
                };
            }
        }
    }
}
