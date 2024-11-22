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
    public class AdminLogicTests
    {
        private readonly Mock<IAdminStorage> _adminStorageMock;
        private readonly Mock<ILogger<AdminLogic>> _loggerMock;
        private readonly AdminLogic _adminLogic;

        public AdminLogicTests()
        {
            _adminStorageMock = new Mock<IAdminStorage>();
            _loggerMock = new Mock<ILogger<AdminLogic>>();
            _adminLogic = new AdminLogic(_loggerMock.Object, _adminStorageMock.Object);
        }

        [Fact]
        public void Create_ValidModel_ReturnsTrue()
        {
            var model = new AdminBindingModel
            {
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Password = "password123!"
            };

            _adminStorageMock.Setup(s => s.Insert(model)).Returns(new AdminViewModel { Id = 1 });

            var result = _adminLogic.Create(model);

            Assert.True(result);
            _adminStorageMock.Verify(s => s.Insert(It.IsAny<AdminBindingModel>()), Times.Once);
        }

        [Fact]
        public void Create_InvalidPassword_ThrowsArgumentException()
        {
            var model = new AdminBindingModel
            {
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Password = "password123"
            };

            var ex = Assert.Throws<ArgumentException>(() => _adminLogic.Create(model));
            Assert.Equal("Неправильно введенный пароль (Parameter 'Password')", ex.Message);
            _adminStorageMock.Verify(s => s.Insert(It.IsAny<AdminBindingModel>()), Times.Never);
        }

        [Fact]
        public void Delete_ValidModel_ReturnsTrue()
        {
            var model = new AdminBindingModel { Id = 1 };

            _adminStorageMock.Setup(s => s.Delete(model)).Returns(new AdminViewModel { Id = 1 });

            var result = _adminLogic.Delete(model);

            Assert.True(result);
            _adminStorageMock.Verify(s => s.Delete(It.IsAny<AdminBindingModel>()), Times.Once);
        }

        [Fact]
        public void ReadElement_ValidSearchModel_ReturnsAdminViewModel()
        {
            var searchModel = new AdminSearchModel { Id = 1 };
            var expectedAdmin = new AdminViewModel { Id = 1, Name = "Иван", Surname = "Иванов", Patronymic = "Иванович" };

            _adminStorageMock.Setup(s => s.GetElement(searchModel)).Returns(expectedAdmin);

            var result = _adminLogic.ReadElement(searchModel);

            Assert.NotNull(result);
            Assert.Equal(expectedAdmin.Id, result.Id);
            Assert.Equal(expectedAdmin.Name, result.Name);
            Assert.Equal(expectedAdmin.Surname, result.Surname);
            Assert.Equal(expectedAdmin.Patronymic, result.Patronymic);

            _adminStorageMock.Verify(s => s.GetElement(It.IsAny<AdminSearchModel>()), Times.Once);
        }

        [Fact]
        public void ReadList_WithNullSearchModel_ReturnsAllAdmins()
        {
            var admins = new List<AdminViewModel>
            {
                new AdminViewModel { Id = 1, Name = "Иван", Surname = "Иванов", Patronymic = "Иванович" },
                new AdminViewModel { Id = 2, Name = "Петр", Surname = "Петров", Patronymic = "Петрович" }
            };

            _adminStorageMock.Setup(s => s.GetAll()).Returns(admins);

            var result = _adminLogic.ReadList(null);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _adminStorageMock.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public void Update_ValidModel_ReturnsTrue()
        {
            var model = new AdminBindingModel
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Password = "password1!"
            };

            _adminStorageMock.Setup(s => s.Update(model)).Returns(new AdminViewModel { Id = 1 });

            var result = _adminLogic.Update(model);

            Assert.True(result);
            _adminStorageMock.Verify(s => s.Update(It.IsAny<AdminBindingModel>()), Times.Once);
        }
    }
}
