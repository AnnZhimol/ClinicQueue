using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class ElectronicQueueStorage : IElectronicQueueStorage
    {
        public ElectronicQueueViewModel? Delete(ElectronicQueueBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var element = context.ElectronicQueues
                .Include(x => x.Doctor)
                .Include(x => x.Admin)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.ElectronicQueues.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public List<ElectronicQueueViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();

            return context.ElectronicQueues
                .Include(x => x.Doctor)
                .Include(x => x.Admin)
                .Select(x => x.GetViewModel).ToList();
        }

        public ElectronicQueueViewModel? GetElement(ElectronicQueueSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            return context.ElectronicQueues
                    .Include(x => x.Doctor)
                    .Include(x => x.Admin)
                    .FirstOrDefault(x =>
                        (model.Status == null || model.Status != null) &&
                        model.DoctorId.HasValue && x.DoctorId == model.DoctorId ||
                        model.Id.HasValue && x.Id == model.Id ||
                        model.AdminId.HasValue && x.AdminId == model.AdminId
                    )
                    ?.GetViewModel;
        }

        public List<ElectronicQueueViewModel> GetFilteredAll(ElectronicQueueSearchModel model)
        {
            if (model.Id.HasValue)
            {
                var result = GetElement(model);
                return result != null ? new() { result } : new();
            }

            using var context = new ClinicQueueDataBase();
            IQueryable<ElectronicQueue>? queryWhere = null;

            if (model.Status != null)
            {
                queryWhere = context.ElectronicQueues.Where(x => model.Status.Equals(x.Status));
            }

            else if (model.DoctorId.HasValue)
            {
                queryWhere = context.ElectronicQueues.Where(x => x.DoctorId == model.DoctorId);
            }

            else if (model.AdminId.HasValue)
            {
                queryWhere = context.ElectronicQueues.Where(x => x.AdminId == model.AdminId);
            }

            else
            {
                return new();
            }

            return queryWhere
                    .Include(x => x.Admin)
                    .Include(x => x.Doctor)
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public ElectronicQueueViewModel? Insert(ElectronicQueueBindingModel model)
        {
            var newElectronicQueue = ElectronicQueue.Create(model);

            if (newElectronicQueue == null)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            context.ElectronicQueues.Add(newElectronicQueue);
            context.SaveChanges();

            return context.ElectronicQueues
                          .Include(x => x.Admin)
                          .Include(x => x.Doctor)
                          .FirstOrDefault(x => x.Id == newElectronicQueue.Id)
                          ?.GetViewModel;
        }

        public ElectronicQueueViewModel? Update(ElectronicQueueBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var order = context.ElectronicQueues
                .Include(x => x.Admin)
                .Include(x => x.Doctor)
                .FirstOrDefault(x => x.Id == model.Id);

            if (order == null)
            {
                return null;
            }

            order.Update(model);
            context.SaveChanges();

            return order.GetViewModel;
        }
    }
}
