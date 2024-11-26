using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class DoctorStorage : IDoctorStorage
    {
        public DoctorViewModel? Delete(DoctorBindingModel model)
        {
            using var context = new ClinicQueueDataBase();
            var element = context.Doctors.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Doctors.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public List<DoctorViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();
            return context.Doctors.OrderBy(x => x.Surname).Select(x => x.GetViewModel).ToList();
        }

        public DoctorViewModel? GetElement(DoctorSearchModel model)
        {
            using var context = new ClinicQueueDataBase();
            if (model.Id.HasValue)
                return context.Doctors.FirstOrDefault(x => x.Id == model.Id)?.GetViewModel;

            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname)
                && !string.IsNullOrEmpty(model.Patronymic) && !string.IsNullOrEmpty(model.Password))
                return context.Doctors.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Patronymic.Equals(model.Patronymic) && x.Password.Equals(model.Password))?.GetViewModel;
            
            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname)
                && string.IsNullOrEmpty(model.Patronymic) && !string.IsNullOrEmpty(model.Password))
                return context.Doctors.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Patronymic.Equals(model.Patronymic) && x.Password.Equals(model.Password))?.GetViewModel;
            return null;
        }

        public List<DoctorViewModel> GetFilteredAll(DoctorSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Specialization))
            {
                return new();
            }
            using var context = new ClinicQueueDataBase();
            return context.Doctors.Where(x => x.Specialization.Contains(model.Specialization)).Select(x => x.GetViewModel).ToList();
        }

        public DoctorViewModel? Insert(DoctorBindingModel model)
        {
            var newDoctor = Doctor.Create(model);
            if (newDoctor == null)
            {
                return null;
            }
            using var context = new ClinicQueueDataBase();
            context.Doctors.Add(newDoctor);
            context.SaveChanges();
            return newDoctor.GetViewModel;
        }

        public DoctorViewModel? Update(DoctorBindingModel model)
        {
            using var context = new ClinicQueueDataBase();
            var doctor = context.Doctors.FirstOrDefault(x => x.Id == model.Id);
            if (doctor == null)
            {
                return null;
            }
            doctor.Update(model);
            context.SaveChanges();
            return doctor.GetViewModel;
        }
    }
}
