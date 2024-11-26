using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ClinicQueueTest.ModuleTests
{
    public class ElectronicQueueLogicTests
    {
        private readonly Mock<ILogger<ElectronicQueueLogic>> _loggerMock;
        private readonly Mock<IElectronicQueueStorage> _storageMock;
        private readonly ElectronicQueueLogic _logic;

        public ElectronicQueueLogicTests()
        {
            _loggerMock = new Mock<ILogger<ElectronicQueueLogic>>();
            _storageMock = new Mock<IElectronicQueueStorage>();
            _logic = new ElectronicQueueLogic(_loggerMock.Object, _storageMock.Object);
        }

        [Fact]
        public void Create_ShouldReturnQueue_WhenValidModelIsPassed()
        {
            var model = new ElectronicQueueBindingModel
            {
                Id = 1,
                Name = "Test Queue",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                DoctorId = 1,
                AdminId = 1
            };

            var expectedQueue = new ElectronicQueueViewModel
            {
                Id = 1,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = ElectronicQueueStatus.Активна,
                DoctorId = model.DoctorId,
                AdminId = model.AdminId
            };

            _storageMock.Setup(s => s.Insert(It.IsAny<ElectronicQueueBindingModel>())).Returns(expectedQueue);

            var result = _logic.Create(model);

            Assert.NotNull(result);
            Assert.Equal(expectedQueue.Id, result.Id);
            _storageMock.Verify(s => s.Insert(It.IsAny<ElectronicQueueBindingModel>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _logic.Create(null));
        }

        [Fact]
        public void IsCompleted_ShouldReturnTrue_WhenQueueAlreadyCompleted()
        {
            var model = new ElectronicQueueBindingModel { Id = 1 };
            var existingQueue = new ElectronicQueueViewModel
            {
                Id = 1,
                Status = ElectronicQueueStatus.Завершена
            };

            _storageMock.Setup(s => s.GetElement(It.IsAny<ElectronicQueueSearchModel>())).Returns(existingQueue);

            var result = _logic.isCompleted(model);

            Assert.True(result);
            _storageMock.Verify(s => s.Update(It.IsAny<ElectronicQueueBindingModel>()), Times.Never);
        }

        [Fact]
        public void IsCompleted_ShouldReturnFalse_WhenQueueNotFound()
        {
            var model = new ElectronicQueueBindingModel { Id = 1 };
            _storageMock.Setup(s => s.GetElement(It.IsAny<ElectronicQueueSearchModel>())).Returns((ElectronicQueueViewModel)null);

            var result = _logic.isCompleted(model);

            Assert.False(result);
        }

        [Fact]
        public void IsCompleted_ShouldUpdateQueue_WhenQueueNotCompleted()
        {
            var model = new ElectronicQueueBindingModel { Id = 1 };
            var existingQueue = new ElectronicQueueViewModel
            {
                Id = 1,
                Status = ElectronicQueueStatus.Активна
            };

            _storageMock.Setup(s => s.GetElement(It.IsAny<ElectronicQueueSearchModel>())).Returns(existingQueue);
            _storageMock.Setup(s => s.Update(It.IsAny<ElectronicQueueBindingModel>())).Returns(new ElectronicQueueViewModel());

            var result = _logic.isCompleted(model);

            Assert.True(result);
            _storageMock.Verify(s => s.Update(It.IsAny<ElectronicQueueBindingModel>()), Times.Once);
        }

        [Fact]
        public void ReadElement_ShouldReturnQueue_WhenQueueExists()
        {
            var model = new ElectronicQueueSearchModel { Id = 1 };
            var expectedQueue = new ElectronicQueueViewModel
            {
                Id = 1,
                Name = "Test Queue",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Status = ElectronicQueueStatus.Активна
            };

            _storageMock.Setup(s => s.GetElement(It.IsAny<ElectronicQueueSearchModel>())).Returns(expectedQueue);

            var result = _logic.ReadElement(model);

            Assert.NotNull(result);
            Assert.Equal(expectedQueue.Id, result.Id);
        }

        [Fact]
        public void ReadElement_ShouldReturnNull_WhenQueueNotFound()
        {
            var model = new ElectronicQueueSearchModel { Id = 1 };
            _storageMock.Setup(s => s.GetElement(It.IsAny<ElectronicQueueSearchModel>())).Returns((ElectronicQueueViewModel)null);

            var result = _logic.ReadElement(model);

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenValidModelIsPassed()
        {
            var model = new ElectronicQueueBindingModel
            {
                Id = 1,
                Name = "Updated Queue",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                DoctorId = 1,
                AdminId = 1
            };
            var modelView = new ElectronicQueueViewModel
            {
                Id = 1,
                Name = "Updated Queue",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                DoctorId = 1,
                AdminId = 1
            };

            _storageMock.Setup(s => s.Update(It.IsAny<ElectronicQueueBindingModel>())).Returns(modelView);

            var result = _logic.Update(model);

            Assert.True(result);
            _storageMock.Verify(s => s.Update(It.IsAny<ElectronicQueueBindingModel>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowArgumentNullException_WhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _logic.Update(null));
        }
    }
}
