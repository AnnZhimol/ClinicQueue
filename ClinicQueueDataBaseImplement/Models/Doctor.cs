﻿using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class Doctor : IDoctorModel
    {
        [Required]
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string Surname { get; set; } = string.Empty;
        [DataMember]
        public string? Patronymic { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string Specialization { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataMember]
        public string CabinetNumber { get; set; } = string.Empty;
        [Key]
        [DataMember]
        public int Id { get; set; }
        [ForeignKey("DoctorId")]
        public virtual List<Appointment> Appointments { get; set; } = new();
        [ForeignKey("DoctorId")]
        public virtual List<ElectronicQueue> ElectronicQueue { get; set; } = new();
        public static Doctor? Create(DoctorBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Doctor()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Password = model.Password,
                Specialization = model.Specialization,
                CabinetNumber = model.CabinetNumber
            };
        }
        public static Doctor Create(DoctorViewModel model)
        {
            return new Doctor
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Password = model.Password,
                Specialization = model.Specialization,
                CabinetNumber = model.CabinetNumber
            };
        }
        public void Update(DoctorBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Name = model.Name;
            Surname = model.Surname;
            Patronymic = model.Patronymic;
            Password = model.Password;
            Specialization = model.Specialization;
            CabinetNumber = model.CabinetNumber;
        }
        public DoctorViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            Specialization= Specialization,
            CabinetNumber = CabinetNumber,
            Password = Password
        };
    }
}
