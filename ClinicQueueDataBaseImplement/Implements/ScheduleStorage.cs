using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class ScheduleStorage : IScheduleStorage
    {
        public ScheduleViewModel? Delete(ScheduleBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var element = context.Schedules
                .Include(x => x.Doctor)
                .Include(x => x.Admin)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Schedules.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public List<ScheduleViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();

            return context.Schedules
                .Include(x => x.Doctor)
                .Include(x => x.Admin)
                .Select(x => x.GetViewModel).ToList();
        }

        public ScheduleViewModel? GetElement(ScheduleSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            return context.Schedules
                    .Include(x => x.Doctor)
                    .Include(x => x.Admin)
                    .FirstOrDefault(x =>
                        model.DoctorId.HasValue && x.DoctorId == model.DoctorId ||
                        model.Id.HasValue && x.Id == model.Id ||
                        model.AdminId.HasValue && x.AdminId == model.AdminId
                    )
                    ?.GetViewModel;
        }

        public List<ScheduleViewModel> GetFilteredAll(ScheduleSearchModel model)
        {
            if (model.Id.HasValue)
            {
                var result = GetElement(model);
                return result != null ? new() { result } : new();
            }

            using var context = new ClinicQueueDataBase();
            IQueryable<Schedule>? queryWhere = null;

            if (model.DoctorId.HasValue)
            {
                queryWhere = context.Schedules.Where(x => x.DoctorId == model.DoctorId);
            }

            else if (model.AdminId.HasValue)
            {
                queryWhere = context.Schedules.Where(x => x.AdminId == model.AdminId);
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

        public ScheduleViewModel? Insert(ScheduleBindingModel model)
        {
            var newSchedule = Schedule.Create(model);

            if (newSchedule == null)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            context.Schedules.Add(newSchedule);
            context.SaveChanges();

            return context.Schedules
                          .Include(x => x.Admin)
                          .Include(x => x.Doctor)
                          .FirstOrDefault(x => x.Id == newSchedule.Id)
                          ?.GetViewModel;
        }

        public ScheduleViewModel? Update(ScheduleBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var order = context.Schedules
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
