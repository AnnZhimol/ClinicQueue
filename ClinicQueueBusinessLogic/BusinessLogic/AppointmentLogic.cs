using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using Microsoft.Extensions.Logging;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class AppointmentLogic : IAppointmentLogic
    {
        private readonly ILogger _logger;
        private readonly IAppointmentStorage _appointmentStorage;
        public AppointmentLogic(ILogger<AppointmentLogic> logger, IAppointmentStorage appointmentStorage)
        {
            _logger = logger;
            _appointmentStorage = appointmentStorage;
        }
        public bool CancelAppointment(AppointmentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("CancelAppointment. Id: {Id}", model.Id);
            var appointment = _appointmentStorage.GetElement(new AppointmentSearchModel { Id = model.Id });
            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found");
                return false;
            }

            if (appointment.Status == AppointmentStatus.Завершен)
            {
                _logger.LogWarning("Cannot cancel completed appointment");
                return false;
            }

            appointment.Status = AppointmentStatus.Отменен;
            _appointmentStorage.Update(ConvertToBindingModel(appointment));
            return true;
        }

        public bool Create(AppointmentBindingModel model)
        {
            CheckModel(model);
            if (_appointmentStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(AppointmentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id: {Id}", model.Id);
            if (_appointmentStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public bool inProcessing(AppointmentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("inProcessing. Id: {Id}", model.Id);
            var appointment = _appointmentStorage.GetElement(new AppointmentSearchModel { Id = model.Id });
            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found");
                return false;
            }

            if (appointment.Status != AppointmentStatus.Ожидание)
            {
                _logger.LogWarning("Invalid status transition from {Status}", appointment.Status);
                return false;
            }

            appointment.Status = AppointmentStatus.Обработка;
            _appointmentStorage.Update(ConvertToBindingModel(appointment));
            return true;
        }

        public bool inWaiting(AppointmentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("inWaiting. Id: {Id}", model.Id);
            var appointment = _appointmentStorage.GetElement(new AppointmentSearchModel { Id = model.Id });
            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found");
                return false;
            }

            if (appointment.Status != AppointmentStatus.Забронирован)
            {
                _logger.LogWarning("Invalid status transition from {Status}", appointment.Status);
                return false;
            }

            appointment.Status = AppointmentStatus.Ожидание;
            _appointmentStorage.Update(ConvertToBindingModel(appointment));
            return true;
        }

        public bool isCompleted(AppointmentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("isCompleted. Id: {Id}", model.Id);
            var appointment = _appointmentStorage.GetElement(new AppointmentSearchModel { Id = model.Id });
            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found");
                return false;
            }

            if (appointment.Status != AppointmentStatus.Обработка)
            {
                _logger.LogWarning("Invalid status transition from {Status}", appointment.Status);
                return false;
            }

            appointment.Status = AppointmentStatus.Завершен;
            _appointmentStorage.Update(ConvertToBindingModel(appointment));
            return true;
        }

        public AppointmentViewModel? ReadElement(AppointmentSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Appointment. Id: {Id}.", model.Id);
            var element = _appointmentStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);
            return element;
        }

        public List<AppointmentViewModel>? ReadList(AppointmentSearchModel? model)
        {
            _logger.LogInformation("ReadElement. Appointment. Id: {Id}.", model.Id);
            var list = model == null ? _appointmentStorage.GetAll() : _appointmentStorage.GetFilteredAll(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count: {Count}", list.Count);
            return list;
        }

        public bool ReserveAppointment(AppointmentBindingModel model)
        {
            CheckModel(model);
            _logger.LogInformation("ReserveAppointment. Id: {Id}", model.Id);

            var appointmentViewModel = _appointmentStorage.GetElement(new AppointmentSearchModel { Id = model.Id });
            if (appointmentViewModel == null)
            {
                _logger.LogWarning("Appointment not found");
                return false;
            }

            if (appointmentViewModel.Status != AppointmentStatus.Создан)
            {
                _logger.LogWarning("Cannot reserve appointment. Current status: {Status}", appointmentViewModel.Status);
                return false;
            }

            Random random = new Random();
            int reservationNumber;
            do
            {
                reservationNumber = random.Next(100000, 1000000);
            } while (_appointmentStorage.ReservationNumberExists(reservationNumber));

            appointmentViewModel.ReservationNumber = reservationNumber;
            appointmentViewModel.PatientId = model.PatientId;

            appointmentViewModel.Status = AppointmentStatus.Забронирован;

            if (_appointmentStorage.Update(ConvertToBindingModel(appointmentViewModel)) == null)
            {
                _logger.LogWarning("Failed to update appointment with reservation number.");
                return false;
            }

            return true;
        }

        public bool Update(AppointmentBindingModel model)
        {
            CheckModel(model);
            if (_appointmentStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(AppointmentBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }
            if (model.AppointmentStart == default)
            {
                throw new ArgumentNullException("Не указана дата приема", nameof(model.AppointmentStart));
            }

            if (model.DoctorId <= 0)
            {
                throw new ArgumentNullException("Не указан доктор", nameof(model.DoctorId));
            }

            _logger.LogInformation("Appointment. Date: {Date}. DoctorId: {DoctorId}.", model.AppointmentStart, model.DoctorId);
        }
        private AppointmentBindingModel ConvertToBindingModel(AppointmentViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "AppointmentViewModel cannot be null");
            }

            return new AppointmentBindingModel
            {
                Id = viewModel.Id,
                ReservationNumber = viewModel.ReservationNumber,
                AppointmentStart = viewModel.AppointmentStart,
                Status = viewModel.Status,
                PatientId = viewModel.PatientId,
                DoctorId = viewModel.DoctorId,
                ElectronicQueueId = viewModel.ElectronicQueueId
            };
        }
    }
}
