using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class AdminLogic : IAdminLogic
    {
        private readonly ILogger _logger;
        private readonly IAdminStorage _adminStorage;
        public AdminLogic(ILogger<AdminLogic> logger, IAdminStorage adminStorage)
        {
            _logger = logger;
            _adminStorage = adminStorage;
        }
        public bool Create(AdminBindingModel model)
        {
            CheckModel(model);
            if (_adminStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(AdminBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id: {Id}", model.Id);
            if (_adminStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public AdminViewModel? ReadElement(AdminSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Name: {Name}. Surname: {Surname}. Patronymic: {Patronymic}. Id: {Id}.", model.Name, model.Surname, model.Patronymic, model.Id);
            var element = _adminStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);
            return element;
        }

        public List<AdminViewModel>? ReadList(AdminSearchModel? model)
        {
            var list = model == null ? _adminStorage.GetAll() : _adminStorage.GetFilteredAll(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count: {Count}", list.Count);
            return list;
        }

        public bool Update(AdminBindingModel model)
        {
            CheckModel(model);
            if (_adminStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(AdminBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Нет имени админа", nameof(model.Name));
            }

            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new ArgumentNullException("Нет фамилии админа", nameof(model.Surname));
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentNullException("Нет пароля админа", nameof(model.Password));
            }

            if (!Regex.IsMatch(model.Password, @"^^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("Неправильно введенный пароль", nameof(model.Password));
            }

            _logger.LogInformation("Admin. Name: {Name}. Surname: {Surname}. Patronymic: {Patronymic}. Id: {Id}", model.Name, model.Surname, model.Patronymic, model.Id);

            var element = _adminStorage.GetElement(new AdminSearchModel
            {
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Админ с такими данными уже есть");
            }
        }
    }
}
