using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
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
    public class AppointmentLogicTests
    {
        private readonly Mock<IAppointmentStorage> _appointmentStorageMock;
        private readonly Mock<ILogger<AppointmentLogic>> _loggerMock;
        private readonly AppointmentLogic _appointmentLogic;

        public AppointmentLogicTests()
        {
            _appointmentStorageMock = new Mock<IAppointmentStorage>();
            _loggerMock = new Mock<ILogger<AppointmentLogic>>();
            _appointmentLogic = new AppointmentLogic(_loggerMock.Object, _appointmentStorageMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnTrue_WhenSuccessful()
        {
            var model = new AppointmentViewModel
            {
                Id = 1,
                AppointmentStart = DateTime.Now.AddHours(1),
                DoctorId = 1,
                PatientId = 1,
                ElectronicQueueId = 1
            };
            var modelBinding = new AppointmentBindingModel
            {
                Id = 1,
                AppointmentStart = DateTime.Now.AddHours(1),
                DoctorId = 1,
                PatientId = 1,
                ElectronicQueueId = 1
            };
            _appointmentStorageMock.Setup(s => s.Insert(It.IsAny<AppointmentBindingModel>())).Returns(model);

            var result = _appointmentLogic.Create(modelBinding);

            Assert.True(result);
            _appointmentStorageMock.Verify(s => s.Insert(It.IsAny<AppointmentBindingModel>()), Times.Once);
        }
        [Fact]
        public void CancelAppointment_ShouldReturnTrue_WhenSuccessful()
        {
            var model = new AppointmentBindingModel { Id = 1 };
            var modelView = new AppointmentViewModel { Id = 1 };
            var appointment = new AppointmentViewModel
            {
                Id = 1,
                Status = AppointmentStatus.Создан
            };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns(appointment);
            _appointmentStorageMock.Setup(s => s.Update(It.IsAny<AppointmentBindingModel>())).Returns(modelView);

            var result = _appointmentLogic.CancelAppointment(model);

            Assert.True(result);
            _appointmentStorageMock.Verify(s => s.Update(It.IsAny<AppointmentBindingModel>()), Times.Once);
        }

        [Fact]
        public void CancelAppointment_ShouldReturnFalse_WhenAppointmentNotFound()
        {
            var model = new AppointmentBindingModel { Id = 1 };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns((AppointmentViewModel)null);

            var result = _appointmentLogic.CancelAppointment(model);

            Assert.False(result);
        }

        [Fact]
        public void CancelAppointment_ShouldReturnFalse_WhenAppointmentIsCompleted()
        {
            var model = new AppointmentBindingModel { Id = 1 };
            var appointment = new AppointmentViewModel
            {
                Id = 1,
                Status = AppointmentStatus.Завершен
            };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns(appointment);

            var result = _appointmentLogic.CancelAppointment(model);

            Assert.False(result);
        }
        [Fact]
        public void inProcessing_ShouldReturnTrue_WhenSuccessful()
        {
            var model = new AppointmentBindingModel { Id = 1 };
            var modelView = new AppointmentViewModel { Id = 1 };
            var appointment = new AppointmentViewModel
            {
                Id = 1,
                Status = AppointmentStatus.Ожидание
            };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns(appointment);
            _appointmentStorageMock.Setup(s => s.Update(It.IsAny<AppointmentBindingModel>())).Returns(modelView);

            var result = _appointmentLogic.inProcessing(model);

            Assert.True(result);
            _appointmentStorageMock.Verify(s => s.Update(It.IsAny<AppointmentBindingModel>()), Times.Once);
        }

        [Fact]
        public void inProcessing_ShouldReturnFalse_WhenAppointmentNotFound()
        {
            var model = new AppointmentBindingModel { Id = 1 };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns((AppointmentViewModel)null);

            var result = _appointmentLogic.inProcessing(model);

            Assert.False(result);
        }

        [Fact]
        public void inProcessing_ShouldReturnFalse_WhenStatusIsNotWaiting()
        {
            var model = new AppointmentBindingModel { Id = 1 };
            var appointment = new AppointmentViewModel
            {
                Id = 1,
                Status = AppointmentStatus.Создан
            };

            _appointmentStorageMock.Setup(s => s.GetElement(It.IsAny<AppointmentSearchModel>())).Returns(appointment);

            var result = _appointmentLogic.inProcessing(model);

            Assert.False(result);
        }
        [Fact]
        public void ReadElement_ShouldReturnAppointment_WhenFound()
        {
            var searchModel = new AppointmentSearchModel { Id = 1 };
            var appointment = new AppointmentViewModel
            {
                Id = 1,
                Status = AppointmentStatus.Создан
            };

            _appointmentStorageMock.Setup(s => s.GetElement(searchModel)).Returns(appointment);

            var result = _appointmentLogic.ReadElement(searchModel);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void ReadElement_ShouldReturnNull_WhenNotFound()
        {
            var searchModel = new AppointmentSearchModel { Id = 1 };

            _appointmentStorageMock.Setup(s => s.GetElement(searchModel)).Returns((AppointmentViewModel)null);

            var result = _appointmentLogic.ReadElement(searchModel);

            Assert.Null(result);
        }
        [Fact]
        public void Update_ShouldReturnTrue_WhenSuccessful()
        {
            var model = new AppointmentBindingModel
            {
                Id = 1,
                AppointmentStart = DateTime.Now.AddHours(3),
                DoctorId = 1,
                PatientId = 1,
                ElectronicQueueId = 1
            };
            var updatedAppointment = new AppointmentBindingModel
            {
                Id = 1,
                Status = AppointmentStatus.Обработка,
                AppointmentStart = DateTime.Now.AddHours(1),
                DoctorId = 1,
                PatientId = 1,
                ElectronicQueueId = 1
            };
            var updatedAppointmentView = new AppointmentViewModel
            {
                Id = 1,
                AppointmentStart = DateTime.Now.AddHours(1),
                Status = AppointmentStatus.Обработка,
                DoctorId = 1,
                PatientId = 1,
                ElectronicQueueId = 1
            };

            _appointmentStorageMock.Setup(s => s.Update(It.IsAny<AppointmentBindingModel>())).Returns(updatedAppointmentView);

            var result = _appointmentLogic.Update(model);

            Assert.True(result);
            _appointmentStorageMock.Verify(s => s.Update(It.IsAny<AppointmentBindingModel>()), Times.Once);
        }
    }
}
