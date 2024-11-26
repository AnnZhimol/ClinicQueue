using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class Appointment : IAppointmentModel
    {
        [DataMember]
        public int? ReservationNumber { get; set; }
        [Required]
        [DataMember]
        public DateTime AppointmentStart { get; set; }
        [Required]
        [DataMember]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Создан;
        [DataMember]
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        [DataMember]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        [DataMember]
        public int ElectronicQueueId { get; set; }
        public virtual ElectronicQueue ElectronicQueue { get; set; }
        [Key]
        [DataMember]
        public int Id { get; set; }
        public static Appointment? Create(AppointmentBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }

            return new Appointment()
            {
                Id = model.Id,
                ReservationNumber = model.ReservationNumber,
                AppointmentStart = model.AppointmentStart,
                Status = model.Status,
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                ElectronicQueueId = model.ElectronicQueueId
            };
        }

        public void Update(AppointmentBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            ReservationNumber = model.ReservationNumber;
            Status = model.Status;
            PatientId = model.PatientId;
        }

        public AppointmentViewModel GetViewModel
        {
            get
            {
                using var context = new ClinicQueueDataBase();
                var patient = context.Patients.FirstOrDefault(x => x.Id == PatientId);
                var doctor = context.Doctors.FirstOrDefault(x => x.Id == DoctorId);
                return new AppointmentViewModel
                {
                    Id = Id,
                    DoctorId = DoctorId,
                    Status = Status,
                    ElectronicQueueId= ElectronicQueueId,
                    PatientId = PatientId,
                    AppointmentStart = AppointmentStart,
                    ReservationNumber = ReservationNumber,
                    PatientFIO = patient != null
                        ? $"{patient.Surname} {patient.Name} {patient.Patronymic}"
                        : string.Empty,
                    DoctorFIO = doctor != null
                        ? $"{doctor.Surname} {doctor.Name} {doctor.Patronymic}"
                        : string.Empty,
                    ElectronicQueueName = context.ElectronicQueues.FirstOrDefault(x => x.Id == ElectronicQueueId)?.Name ?? string.Empty
                };
            }
        }
    }
}
