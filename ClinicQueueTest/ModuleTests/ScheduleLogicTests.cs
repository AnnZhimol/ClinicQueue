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
    public class ScheduleLogicTests
    {
        private readonly Mock<ILogger<ScheduleLogic>> _mockLogger;
        private readonly Mock<IScheduleStorage> _mockScheduleStorage;
        private readonly ScheduleLogic _scheduleLogic;

        public ScheduleLogicTests()
        {
            _mockLogger = new Mock<ILogger<ScheduleLogic>>();
            _mockScheduleStorage = new Mock<IScheduleStorage>();
            _scheduleLogic = new ScheduleLogic(_mockLogger.Object, _mockScheduleStorage.Object);
        }

        [Fact]
        public void Create_ShouldReturnFalse_WhenModelIsInvalid()
        {
            var model = new ScheduleBindingModel
            {
                Id = 1,
                DateOfWeek = string.Empty,
                Time = default,
                DoctorId = 0,
                AdminId = 0
            };

            var result = _scheduleLogic.Create(model);

            Assert.False(result);
        }

        [Fact]
        public void Create_ShouldReturnTrue_WhenModelIsValid()
        {
            var model = new ScheduleBindingModel
            {
                Id = 1,
                DateOfWeek = "Monday",
                Time = new TimeOnly(9, 0),
                DoctorId = 1,
                AdminId = 1
            };
            var modelView = new ScheduleViewModel
            {
                Id = 1,
                DateOfWeek = "Monday",
                Time = new TimeOnly(9, 0),
                DoctorId = 1,
                AdminId = 1
            };

            _mockScheduleStorage.Setup(ss => ss.Insert(It.IsAny<ScheduleBindingModel>())).Returns(modelView);

            var result = _scheduleLogic.Create(model);

            Assert.True(result);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenScheduleNotFound()
        {
            var model = new ScheduleBindingModel { Id = 1 };
            _mockScheduleStorage.Setup(ss => ss.GetElement(It.IsAny<ScheduleSearchModel>())).Returns((ScheduleViewModel?)null);

            var result = _scheduleLogic.Delete(model);

            Assert.False(result);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenScheduleDeletedSuccessfully()
        {
            var model = new ScheduleBindingModel { Id = 1 };
            var modelView = new ScheduleViewModel { Id = 1 };
            _mockScheduleStorage.Setup(ss => ss.GetElement(It.IsAny<ScheduleSearchModel>())).Returns(new ScheduleViewModel());
            _mockScheduleStorage.Setup(ss => ss.Delete(It.IsAny<ScheduleBindingModel>())).Returns(modelView);

            var result = _scheduleLogic.Delete(model);

            Assert.True(result);
        }

        [Fact]
        public void ReadElement_ShouldReturnNull_WhenScheduleNotFound()
        {
            var searchModel = new ScheduleSearchModel { Id = 1 };
            _mockScheduleStorage.Setup(ss => ss.GetElement(searchModel)).Returns((ScheduleViewModel?)null);

            var result = _scheduleLogic.ReadElement(searchModel);

            Assert.Null(result);
        }

        [Fact]
        public void ReadElement_ShouldReturnSchedule_WhenFound()
        {
            var searchModel = new ScheduleSearchModel { Id = 1 };
            var expectedSchedule = new ScheduleViewModel { Id = 1, DateOfWeek = "Monday", Time = new TimeOnly(9, 0) };
            _mockScheduleStorage.Setup(ss => ss.GetElement(searchModel)).Returns(expectedSchedule);

            var result = _scheduleLogic.ReadElement(searchModel);

            Assert.Equal(expectedSchedule, result);
        }

        [Fact]
        public void ReadList_ShouldReturnNull_WhenNoSchedulesFound()
        {
            _mockScheduleStorage.Setup(ss => ss.GetAll()).Returns(new List<ScheduleViewModel>());

            var result = _scheduleLogic.ReadList(null);

            Assert.Null(result);
        }

        [Fact]
        public void ReadList_ShouldReturnSchedules_WhenFound()
        {
            var scheduleList = new List<ScheduleViewModel>
            {
                new ScheduleViewModel { Id = 1, DateOfWeek = "Monday", Time = new TimeOnly(9, 0) },
                new ScheduleViewModel { Id = 2, DateOfWeek = "Tuesday", Time = new TimeOnly(10, 0) }
            };
            _mockScheduleStorage.Setup(ss => ss.GetAll()).Returns(scheduleList);

            var result = _scheduleLogic.ReadList(null);

            Assert.Equal(scheduleList, result);
        }

        [Fact]
        public void Update_ShouldReturnFalse_WhenModelIsInvalid()
        {
            var model = new ScheduleBindingModel
            {
                Id = 1,
                DateOfWeek = string.Empty,
                Time = default,
                DoctorId = 0,
                AdminId = 0
            };

            var result = _scheduleLogic.Update(model);

            Assert.False(result);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            var model = new ScheduleBindingModel
            {
                Id = 1,
                DateOfWeek = "Monday",
                Time = new TimeOnly(9, 0),
                DoctorId = 1,
                AdminId = 1
            };
            var modelView = new ScheduleViewModel
            {
                Id = 1,
                DateOfWeek = "Monday",
                Time = new TimeOnly(9, 0),
                DoctorId = 1,
                AdminId = 1
            };

            _mockScheduleStorage.Setup(ss => ss.Update(It.IsAny<ScheduleBindingModel>())).Returns(modelView);

            var result = _scheduleLogic.Update(model);

            Assert.True(result);
        }
    }
}
