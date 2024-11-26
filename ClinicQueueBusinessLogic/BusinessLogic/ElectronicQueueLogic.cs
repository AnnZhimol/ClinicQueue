using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using Microsoft.Extensions.Logging;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class ElectronicQueueLogic : IElectronicQueueLogic
    {
        private readonly ILogger _logger;
        private readonly IElectronicQueueStorage _electronicQueueStorage;

        public ElectronicQueueLogic(ILogger<ElectronicQueueLogic> logger, IElectronicQueueStorage electronicQueueStorage)
        {
            _logger = logger;
            _electronicQueueStorage = electronicQueueStorage;
        }

        public ElectronicQueueViewModel? Create(ElectronicQueueBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.Status = ElectronicQueueStatus.Активна;
            var queue = _electronicQueueStorage.Insert(model);
            if (queue == null)
            {
                _logger.LogWarning("Insert operation failed");
                return null;
            }
            _logger.LogInformation("Created electronic queue entry with Id: {Id}", model.Id);
            return queue;
        }

        public bool isCompleted(ElectronicQueueBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var existingQueue = _electronicQueueStorage.GetElement(new ElectronicQueueSearchModel { Id = model.Id });
            if (existingQueue == null)
            {
                _logger.LogWarning("Electronic queue not found for Id: {Id}", model.Id);
                return false;
            }

            if (existingQueue.Status == ElectronicQueueStatus.Завершена)
            {
                _logger.LogInformation("Electronic queue with Id: {Id} is already completed", model.Id);
                return true;
            }

            existingQueue.Status = ElectronicQueueStatus.Завершена;
            if (_electronicQueueStorage.Update(ConvertToBindingModel(existingQueue)) == null)
            {
                _logger.LogWarning("Update operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Electronic queue with Id: {Id} has been marked as completed", model.Id);
            return true;
        }

        public ElectronicQueueViewModel? ReadElement(ElectronicQueueSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("Reading electronic queue with Id: {Id}", model.Id);
            var element = _electronicQueueStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("Electronic queue not found for Id: {Id}", model.Id);
                return null;
            }

            _logger.LogInformation("Electronic queue found with Id: {Id}", element.Id);
            return element;
        }

        public List<ElectronicQueueViewModel>? ReadList(ElectronicQueueSearchModel? model)
        {
            _logger.LogInformation("Reading electronic queues list");
            var list = model == null ? _electronicQueueStorage.GetAll() : _electronicQueueStorage.GetFilteredAll(model);
            if (list == null || list.Count == 0)
            {
                _logger.LogWarning("No electronic queues found");
                return new List<ElectronicQueueViewModel>();
            }

            _logger.LogInformation("Found {Count} electronic queues", list.Count);
            return list;
        }

        public bool Update(ElectronicQueueBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (_electronicQueueStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Updated electronic queue entry with Id: {Id}", model.Id);
            return true;
        }
        private bool CheckModel(ElectronicQueueBindingModel model)
        {
            return !string.IsNullOrEmpty(model.Name) &&
                   model.StartDate < model.EndDate &&
                   model.DoctorId > 0 &&
                   model.AdminId > 0;
        }
        private ElectronicQueueBindingModel ConvertToBindingModel(ElectronicQueueViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            return new ElectronicQueueBindingModel
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                Status = viewModel.Status,
                DoctorId = viewModel.DoctorId,
                AdminId = viewModel.AdminId
            };
        }
    }
}
