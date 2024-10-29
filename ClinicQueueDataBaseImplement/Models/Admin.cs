using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ClinicQueueDataBaseImplement.Models
{
    [DataContract]
    public class Admin : IAdminModel
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
        public string Password { get; set; } = string.Empty;
        [Key]
        [DataMember]
        public int Id { get; private set; }
        [ForeignKey("AdminId")]
        public virtual List<ElectronicQueue> ElectronicQueues { get; set; } = new();
        [ForeignKey("AdminId")]
        public virtual List<Schedule> Schedules { get; set; } = new();
        public static Admin? Create(AdminBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Admin()
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Password = model.Password
            };
        }
        public static Admin Create(AdminViewModel model)
        {
            return new Admin
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Password = model.Password
            };
        }
        public void Update(AdminBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Name = model.Name;
            Surname = model.Surname;
            Patronymic = model.Patronymic;
            Password = model.Password;
        }
        public AdminViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            Password = Password
        };
    }
}
