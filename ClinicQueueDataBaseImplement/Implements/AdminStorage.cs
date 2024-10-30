using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class AdminStorage : IAdminStorage
    {
        public AdminViewModel? Delete(AdminBindingModel model)
        {
            using var context = new ClinicQueueDataBase();
            var element = context.Admins.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Admins.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public List<AdminViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();
            return context.Admins.Select(x => x.GetViewModel).ToList();
        }

        public AdminViewModel? GetElement(AdminSearchModel model)
        {
            using var context = new ClinicQueueDataBase();
            if (model.Id.HasValue)
                return context.Admins.FirstOrDefault(x => x.Id == model.Id)?.GetViewModel;
            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname) 
                && !string.IsNullOrEmpty(model.Patronymic) && !string.IsNullOrEmpty(model.Password))
                return context.Admins.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Patronymic.Equals(model.Patronymic) && x.Password.Equals(model.Password))?.GetViewModel;
            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname) && !string.IsNullOrEmpty(model.Password))
                return context.Admins.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Surname.Equals(model.Surname)
                && x.Password.Equals(model.Password))?.GetViewModel;
            return null;
        }

        public List<AdminViewModel> GetFilteredAll(AdminSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Surname) || string.IsNullOrEmpty(model.Name))
            {
                return new();
            }
            using var context = new ClinicQueueDataBase();
            return context.Admins.Where(x => x.Surname.Contains(model.Surname) && x.Name.Contains(model.Name)).Select(x => x.GetViewModel).ToList();
        }

        public AdminViewModel? Insert(AdminBindingModel model)
        {
            var newAdmin = Admin.Create(model);
            if (newAdmin == null)
            {
                return null;
            }
            using var context = new ClinicQueueDataBase();
            context.Admins.Add(newAdmin);
            context.SaveChanges();
            return newAdmin.GetViewModel;
        }

        public AdminViewModel? Update(AdminBindingModel model)
        {
            using var context = new ClinicQueueDataBase();
            var admin = context.Admins.FirstOrDefault(x => x.Id == model.Id);
            if (admin == null)
            {
                return null;
            }
            admin.Update(model);
            context.SaveChanges();
            return admin.GetViewModel;
        }
    }
}
