using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class ScheduleLogic : IScheduleLogic
    {
        private readonly ILogger _logger;
        private readonly IScheduleStorage _scheduleStorage;

        public ScheduleLogic(ILogger<ScheduleLogic> logger, IScheduleStorage scheduleStorage)
        {
            _logger = logger;
            _scheduleStorage = scheduleStorage;
        }

        public bool Create(ScheduleBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                _logger.LogWarning("Model is null or invalid");
                return false;
            }

            if (_scheduleStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            _logger.LogInformation("Created schedule with Id: {Id}", model.Id);
            return true;
        }

        public bool Delete(ScheduleBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var existingSchedule = _scheduleStorage.GetElement(new ScheduleSearchModel { Id = model.Id });
            if (existingSchedule == null)
            {
                _logger.LogWarning("Schedule not found for Id: {Id}", model.Id);
                return false;
            }

            if (_scheduleStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Deleted schedule with Id: {Id}", model.Id);
            return true;
        }

        public ScheduleViewModel? ReadElement(ScheduleSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("Reading schedule with Id: {Id}", model.Id);
            var schedule = _scheduleStorage.GetElement(model);
            if (schedule == null)
            {
                _logger.LogWarning("Schedule not found for Id: {Id}", model.Id);
                return null;
            }

            _logger.LogInformation("Schedule found with Id: {Id}", schedule.Id);
            return schedule;
        }

        public List<ScheduleViewModel>? ReadList(ScheduleSearchModel? model)
        {
            _logger.LogInformation("Reading schedules list");
            var list = model == null ? _scheduleStorage.GetAll() : _scheduleStorage.GetFilteredAll(model);
            if (list == null || list.Count == 0)
            {
                _logger.LogWarning("No schedules found");
                return null;
            }

            _logger.LogInformation("Found {Count} schedules", list.Count);
            return list;
        }

        public bool Update(ScheduleBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                _logger.LogWarning("Model is null or invalid");
                return false;
            }

            if (_scheduleStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Updated schedule with Id: {Id}", model.Id);
            return true;
        }

        private bool CheckModel(ScheduleBindingModel model)
        {
            return !string.IsNullOrEmpty(model.DateOfWeek) &&
                   model.Time != default &&
                   model.AdminId > 0 &&
                   model.DoctorId > 0;
        }
    }
}
