using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace ClinicQueueBusinessLogic.BusinessLogic
{
    public class PatientLogic : IPatientLogic
    {
        private readonly ILogger _logger;
        private readonly IPatientStorage _patientStorage;

        public PatientLogic(ILogger<PatientLogic> logger, IPatientStorage patientStorage)
        {
            _logger = logger;
            _patientStorage = patientStorage;
        }

        public bool Create(PatientBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                _logger.LogWarning("Model is null or invalid");
                return false;
            }

            if (_patientStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            _logger.LogInformation("Created patient with Id: {Id}", model.Id);
            return true;
        }

        public bool Delete(PatientBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var existingPatient = _patientStorage.GetElement(new PatientSearchModel { Id = model.Id });
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient not found for Id: {Id}", model.Id);
                return false;
            }

            if (_patientStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Deleted patient with Id: {Id}", model.Id);
            return true;
        }

        public PatientViewModel? ReadElement(PatientSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("Reading patient with Id: {Id}", model.Id);
            var patient = _patientStorage.GetElement(model);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for Id: {Id}", model.Id);
                return null;
            }

            _logger.LogInformation("Patient found with Id: {Id}", patient.Id);
            return patient;
        }

        public List<PatientViewModel>? ReadList(PatientSearchModel? model)
        {
            _logger.LogInformation("Reading patients list");
            var list = model == null ? _patientStorage.GetAll() : _patientStorage.GetFilteredAll(model);
            if (list == null || list.Count == 0)
            {
                _logger.LogWarning("No patients found");
                return null;
            }

            _logger.LogInformation("Found {Count} patients", list.Count);
            return list;
        }

        public bool Update(PatientBindingModel model)
        {
            if (model == null || !CheckModel(model))
            {
                _logger.LogWarning("Model is null or invalid");
                return false;
            }

            if (_patientStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed for Id: {Id}", model.Id);
                return false;
            }

            _logger.LogInformation("Updated patient with Id: {Id}", model.Id);
            return true;
        }

        private bool CheckModel(PatientBindingModel model)
        {
            return !string.IsNullOrEmpty(model.OMSNumber) &&
                   !string.IsNullOrEmpty(model.Surname) &&
                   !string.IsNullOrEmpty(model.PassportNumber);
        }
    }
}
