using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class PatientStorage : IPatientStorage
    {
        public PatientViewModel? Delete(PatientBindingModel model)
        {
            using var context = new ClinicQueueDataBase();
            var element = context.Patients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Patients.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public List<PatientViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();
            return context.Patients.Select(x => x.GetViewModel).ToList();
        }

        public PatientViewModel? GetElement(PatientSearchModel model)
        {
            using var context = new ClinicQueueDataBase();

            if (model.Id.HasValue)
                return context.Patients.FirstOrDefault(x => x.Id == model.Id)?.GetViewModel;

            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname)
                && !string.IsNullOrEmpty(model.Patronymic) && (!string.IsNullOrEmpty(model.PassportNumber) || !string.IsNullOrEmpty(model.OMSNumber)))
                return context.Patients.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Patronymic.Equals(model.Patronymic) && (x.PassportNumber.Equals(model.PassportNumber) || x.OMSNumber.Equals(model.OMSNumber)))?.GetViewModel;

            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname)
                && string.IsNullOrEmpty(model.Patronymic) && (!string.IsNullOrEmpty(model.PassportNumber) || !string.IsNullOrEmpty(model.OMSNumber)))
                return context.Patients.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Patronymic.Equals(model.Patronymic) && (x.PassportNumber.Equals(model.PassportNumber) || x.OMSNumber.Equals(model.OMSNumber)))?.GetViewModel;
            return null;
        }

        public List<PatientViewModel> GetFilteredAll(PatientSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Surname) || string.IsNullOrEmpty(model.Name))
            {
                return new();
            }
            using var context = new ClinicQueueDataBase();
            return context.Patients.Where(x => x.Surname.Contains(model.Surname) && x.Name.Contains(model.Name)).Select(x => x.GetViewModel).ToList();
        }

        public PatientViewModel? Insert(PatientBindingModel model)
        {
            var newPatient = Patient.Create(model);
            if (newPatient == null)
            {
                return null;
            }
            using var context = new ClinicQueueDataBase();
            context.Patients.Add(newPatient);
            context.SaveChanges();
            return newPatient.GetViewModel;
        }

        public PatientViewModel? Update(PatientBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
