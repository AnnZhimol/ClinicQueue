using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ClinicQueueTest.ModuleTests
{
    public class PatientLogicTests
    {
        private readonly Mock<ILogger<PatientLogic>> _mockLogger;
        private readonly Mock<IPatientStorage> _mockPatientStorage;
        private readonly PatientLogic _patientLogic;

        public PatientLogicTests()
        {
            _mockLogger = new Mock<ILogger<PatientLogic>>();
            _mockPatientStorage = new Mock<IPatientStorage>();
            _patientLogic = new PatientLogic(_mockLogger.Object, _mockPatientStorage.Object);
        }

        [Fact]
        public void Create_ShouldReturnFalse_WhenModelIsInvalid()
        {
            var model = new PatientBindingModel
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                PassportNumber = string.Empty
            };

            var result = _patientLogic.Create(model);

            Assert.False(result);
        }

        [Fact]
        public void Create_ShouldReturnTrue_WhenModelIsValid()
        {
            var model = new PatientBindingModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                PassportNumber = "123456",
                OMSNumber = "1234567812345678"
            };
            var modelView = new PatientViewModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                PassportNumber = "123456",
                OMSNumber = "1234567812345678"
            };

            _mockPatientStorage.Setup(ps => ps.Insert(It.IsAny<PatientBindingModel>())).Returns(modelView);

            var result = _patientLogic.Create(model);

            Assert.True(result);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenPatientNotFound()
        {
            var model = new PatientBindingModel { Id = 1 };
            _mockPatientStorage.Setup(ps => ps.GetElement(It.IsAny<PatientSearchModel>())).Returns((PatientViewModel?)null);

            var result = _patientLogic.Delete(model);

            Assert.False(result);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenPatientDeletedSuccessfully()
        {
            var model = new PatientBindingModel { Id = 1 };
            var modelView = new PatientViewModel { Id = 1 };
            _mockPatientStorage.Setup(ps => ps.GetElement(It.IsAny<PatientSearchModel>())).Returns(new PatientViewModel());
            _mockPatientStorage.Setup(ps => ps.Delete(It.IsAny<PatientBindingModel>())).Returns(modelView);

            var result = _patientLogic.Delete(model);

            Assert.True(result);
        }

        [Fact]
        public void ReadElement_ShouldReturnNull_WhenPatientNotFound()
        {
            var searchModel = new PatientSearchModel { Id = 1 };
            _mockPatientStorage.Setup(ps => ps.GetElement(searchModel)).Returns((PatientViewModel?)null);

            var result = _patientLogic.ReadElement(searchModel);

            Assert.Null(result);
        }

        [Fact]
        public void ReadElement_ShouldReturnPatient_WhenFound()
        {
            var searchModel = new PatientSearchModel { Id = 1 };
            var expectedPatient = new PatientViewModel { Id = 1, Name = "Иван", Surname = "Иванов" };
            _mockPatientStorage.Setup(ps => ps.GetElement(searchModel)).Returns(expectedPatient);

            var result = _patientLogic.ReadElement(searchModel);

            Assert.Equal(expectedPatient, result);
        }

        [Fact]
        public void Update_ShouldReturnFalse_WhenModelIsInvalid()
        {
            var model = new PatientBindingModel
            {
                Id = 1,
                Name = string.Empty,
                Surname = string.Empty,
                PassportNumber = string.Empty
            };

            var result = _patientLogic.Update(model);

            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            var model = new PatientBindingModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                PassportNumber = "123456",
                OMSNumber = "1234567812345678"
            };
            var modelView = new PatientViewModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                PassportNumber = "123456",
                OMSNumber = "1234567812345678"
            };


            _mockPatientStorage.Setup(ps => ps.Update(It.IsAny<PatientBindingModel>())).Returns(modelView);

            var result = _patientLogic.Update(model);

            Assert.True(result);
        }

        [Fact]
        public void ReadList_ShouldReturnNull_WhenNoPatientsFound()
        {
            _mockPatientStorage.Setup(ps => ps.GetAll()).Returns(new List<PatientViewModel>());

            var result = _patientLogic.ReadList(null);

            Assert.Null(result);
        }

        [Fact]
        public void ReadList_ShouldReturnPatients_WhenFound()
        {
            var patientList = new List<PatientViewModel>
            {
                new PatientViewModel { Id = 1, Name = "Иван", Surname = "Иванов" },
                new PatientViewModel { Id = 2, Name = "Jane", Surname = "Иванов" }
            };
            _mockPatientStorage.Setup(ps => ps.GetAll()).Returns(patientList);

            var result = _patientLogic.ReadList(null);

            Assert.Equal(patientList, result);
        }
    }
}
