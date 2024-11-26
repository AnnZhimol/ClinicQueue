using ClinicQueueBusinessLogic.BusinessLogic;
using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.StoragesContracts;
using ClinicQueueContracts.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ClinicQueueTest.ModuleTests
{
    public class DoctorLogicTests
    {
        private readonly Mock<ILogger<DoctorLogic>> _mockLogger;
        private readonly Mock<IDoctorStorage> _mockDoctorStorage;
        private readonly DoctorLogic _doctorLogic;

        public DoctorLogicTests()
        {
            _mockLogger = new Mock<ILogger<DoctorLogic>>();
            _mockDoctorStorage = new Mock<IDoctorStorage>();
            _doctorLogic = new DoctorLogic(_mockLogger.Object, _mockDoctorStorage.Object);
        }

        [Fact]
        public void Create_ShouldReturnTrue_WhenInsertSucceeds()
        {
            var doctor = new DoctorBindingModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Specialization = "Кардиолог",
                Password = "password123!",
                CabinetNumber = "Кабинет_101"
            };
            var doctorView = new DoctorViewModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Specialization = "Кардиолог",
                Password = "password123!",
                CabinetNumber = "Кабинет_101"
            };

            _mockDoctorStorage.Setup(x => x.Insert(It.IsAny<DoctorBindingModel>())).Returns(doctorView);

            var result = _doctorLogic.Create(doctor);

            Assert.True(result);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            var doctor = new DoctorBindingModel { Id = 1 };
            var doctorView = new DoctorViewModel { Id = 1 };

            _mockDoctorStorage.Setup(x => x.Delete(It.IsAny<DoctorBindingModel>())).Returns(doctorView);

            var result = _doctorLogic.Delete(doctor);

            Assert.True(result);
        }

        [Fact]
        public void ReadElement_ShouldReturnDoctor_WhenFound()
        {
            var doctorSearchModel = new DoctorSearchModel { Name = "Иван", Surname = "Иванов" };
            var expectedDoctor = new DoctorViewModel { Id = 1, Name = "Иван", Surname = "Иванов", Specialization = "Кардиолог" };

            _mockDoctorStorage.Setup(x => x.GetElement(It.IsAny<DoctorSearchModel>())).Returns(expectedDoctor);

            var result = _doctorLogic.ReadElement(doctorSearchModel);

            Assert.NotNull(result);
            Assert.Equal(expectedDoctor.Name, result.Name);
        }

        [Fact]
        public void ReadElement_ShouldReturnNull_WhenNotFound()
        {
            var doctorSearchModel = new DoctorSearchModel { Name = "Иван", Surname = "Иванов" };

            _mockDoctorStorage.Setup(x => x.GetElement(It.IsAny<DoctorSearchModel>())).Returns((DoctorViewModel)null);

            var result = _doctorLogic.ReadElement(doctorSearchModel);

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            var doctor = new DoctorBindingModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Specialization = "Кардиолог",
                Password = "qwerty123456!@",
                CabinetNumber = "Кабинет_101"
            };
            var doctorView = new DoctorViewModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Specialization = "Кардиолог",
                Password = "qwerty123456!@",
                CabinetNumber = "Кабинет_101"
            };

            _mockDoctorStorage.Setup(x => x.Update(It.IsAny<DoctorBindingModel>())).Returns(doctorView);

            var result = _doctorLogic.Update(doctor);

            Assert.True(result);
        }
    }
}
