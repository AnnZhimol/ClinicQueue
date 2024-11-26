using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class Schedule : IScheduleModel
    {
        [Required]
        [DataMember]
        public string DateOfWeek { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public TimeOnly Time { get; set; }
        [Required]
        [DataMember]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor {  get; set; }
        [Required]
        [DataMember]
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        [Key]
        [DataMember]
        public int Id { get; set; }
        public static Schedule? Create(ScheduleBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }

            return new Schedule()
            {
                Id = model.Id,
                AdminId = model.AdminId,
                DoctorId = model.DoctorId,
                DateOfWeek = model.DateOfWeek,
                Time = model.Time
            };
        }

        public void Update(ScheduleBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            DateOfWeek = model.DateOfWeek;
            Time = model.Time;
            AdminId = model.AdminId;
        }

        public ScheduleViewModel GetViewModel
        {
            get
            {
                using var context = new ClinicQueueDataBase();
                var admin = context.Admins.FirstOrDefault(x => x.Id == AdminId);
                var doctor = context.Doctors.FirstOrDefault(x => x.Id == DoctorId);
                return new ScheduleViewModel
                {
                    Id = Id,
                    AdminId = AdminId,
                    DoctorId = DoctorId,
                    DateOfWeek = DateOfWeek,
                    Time = Time,
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
