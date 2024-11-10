using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicQueueDataBaseImplement.Implements
{
    public class AppointmentStorage : IAppointmentStorage
    {
        public AppointmentViewModel? Delete(AppointmentBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var element = context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.ElectronicQueue)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Appointments.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public List<AppointmentViewModel> GetAll()
        {
            using var context = new ClinicQueueDataBase();

            return context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.ElectronicQueue)
                .Select(x => x.GetViewModel).ToList();
        }

        public AppointmentViewModel? GetElement(AppointmentSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            return context.Appointments
                    .Include(x => x.Doctor)
                    .Include(x => x.Patient)
                    .Include(x => x.ElectronicQueue) 
                    .FirstOrDefault(x =>
                        (model.Status == null || model.Status != null) &&
                        model.DoctorId.HasValue && x.DoctorId == model.DoctorId ||
                        model.Id.HasValue && x.Id == model.Id ||
                        model.PatientId.HasValue && x.PatientId == model.PatientId ||
                        model.ElectronicQueueId.HasValue && x.ElectronicQueueId == model.ElectronicQueueId ||
                        model.ReservationNumber.HasValue && x.ReservationNumber == model.ReservationNumber
                    )
                    ?.GetViewModel;
        }

        public List<AppointmentViewModel> GetFilteredAll(AppointmentSearchModel model)
        {
            if (model.Id.HasValue)
            {
                var result = GetElement(model);
                return result != null ? new() { result } : new();
            }

            using var context = new ClinicQueueDataBase();
            IQueryable<Appointment>? queryWhere = null;

            if (model.Status != null)
            {
                queryWhere = context.Appointments.Where(x => model.Status.Equals(x.Status));
            }

            else if (model.DoctorId.HasValue)
            {
                queryWhere = context.Appointments.Where(x => x.DoctorId == model.DoctorId);
            }

            else if (model.PatientId.HasValue)
            {
                queryWhere = context.Appointments.Where(x => x.PatientId == model.PatientId);
            }

            else if (model.ElectronicQueueId.HasValue && model.AppointmentStart != null)
            {
                var appointmentStartUtc = model.AppointmentStart.ToUniversalTime();
                queryWhere = context.Appointments.Where(x => x.ElectronicQueueId == model.ElectronicQueueId && x.AppointmentStart >= appointmentStartUtc);
            }

            else
            {
                return new();
            }

            return queryWhere
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Include(x => x.ElectronicQueue)
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public AppointmentViewModel? Insert(AppointmentBindingModel model)
        {
            var newAppointment = Appointment.Create(model);

            if (newAppointment == null)
            {
                return null;
            }

            using var context = new ClinicQueueDataBase();

            context.Appointments.Add(newAppointment);
            context.SaveChanges();

            return context.Appointments
                          .Include(x => x.Patient)
                          .Include(x => x.Doctor)
                          .Include(x => x.ElectronicQueue)
                          .FirstOrDefault(x => x.Id == newAppointment.Id)
                          ?.GetViewModel;
        }

        public bool ReservationNumberExists(int reservationNumber)
        {
            return GetAll().Any(a => a.ReservationNumber == reservationNumber);
        }


        public AppointmentViewModel? Update(AppointmentBindingModel model)
        {
            using var context = new ClinicQueueDataBase();

            var order = context.Appointments
                .Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.ElectronicQueue)
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
