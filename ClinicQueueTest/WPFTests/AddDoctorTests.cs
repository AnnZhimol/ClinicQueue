using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using Xunit;

namespace ClinicQueueTest.WPFTests
{
    [Collection("Sequential")]
    public class AddDoctorTests : IDisposable
    {
        private readonly Application _application;
        private readonly AutomationBase _automation;

        public AddDoctorTests()
        {
            _automation = new UIA3Automation();
            _application = Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        [Fact, TestPriority(1)]
        public void TestAddDoctor_Success()
        {
            var startWindow = _application.GetMainWindow(_automation);

            var employeeButton = startWindow.FindFirstDescendant(cf => cf.ByAutomationId("EmployeeButton")).AsButton();

            employeeButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successStartMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Запуск окна для авторизации сотрудника.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successStartMessageBox);

            var okStartButton = successStartMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okStartButton);
            okStartButton.Invoke();

            var mainWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("MainWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(mainWindow);

            var firstNameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var passwordBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("PasswordBox")).AsTextBox();
            var roleComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("RoleComboBox")).AsComboBox();
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("LoginButton")).AsButton();

            Assert.True(loginButton.IsEnabled, "Login button is not enabled.");
            roleComboBox.Select("Администратор");

            firstNameTextBox.Text = "Ирина";
            lastNameTextBox.Text = "Любова";
            passwordBox.Text = "qwerty123456";

            loginButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Добро пожаловать!")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBox);

            var okButton = successMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okButton);
            okButton.Invoke();

            var adminWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Окно администратора")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(adminWindow);

            var addDoctorButton = adminWindow.FindFirstDescendant(cf => cf.ByAutomationId("AddDoctorButton")).AsButton();
            addDoctorButton.Invoke();
            Thread.Sleep(2000);

            var addDoctorWindow = _application.GetAllTopLevelWindows(_automation)
                                              .FirstOrDefault(w => w.Title.Contains("Добавить нового врача"));
            Assert.NotNull(addDoctorWindow);

            var firstNameTextBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var passwordBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("PasswordBox")).AsTextBox();
            var specializationComboBox = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("SpecializationComboBox")).AsComboBox();
            var roomComboBox = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("RoomNumberComboBox")).AsComboBox();
            var saveButton = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("SaveDoctorButton")).AsButton();

            firstNameTextBoxAdd.Text = "Иван";
            lastNameTextBoxAdd.Text = "Иванов";
            passwordBoxAdd.Text = "qwerty123456!@";
            specializationComboBox.Select("Терапевт");
            roomComboBox.Select("Кабинет_101");

            saveButton.Invoke();

            Thread.Sleep(2000);

            var successMessageBoxAdd = _application.GetAllTopLevelWindows(_automation)
                                                 .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Врач успешно добавлен!")) != null);
            Assert.NotNull(successMessageBoxAdd);
        }

        [Fact, TestPriority(1)]
        public void TestAddDoctor_Failure_MissingFields()
        {
            var startWindow = _application.GetMainWindow(_automation);

            var employeeButton = startWindow.FindFirstDescendant(cf => cf.ByAutomationId("EmployeeButton")).AsButton();

            employeeButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successStartMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Запуск окна для авторизации сотрудника.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successStartMessageBox);

            var okStartButton = successStartMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okStartButton);
            okStartButton.Invoke();

            var mainWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("MainWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(mainWindow);

            var firstNameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var passwordBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("PasswordBox")).AsTextBox();
            var roleComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("RoleComboBox")).AsComboBox();
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("LoginButton")).AsButton();

            Assert.True(loginButton.IsEnabled, "Login button is not enabled.");
            roleComboBox.Select("Администратор");

            firstNameTextBox.Text = "Ирина";
            lastNameTextBox.Text = "Любова";
            passwordBox.Text = "qwerty123456";

            loginButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Добро пожаловать!")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBox);

            var okButton = successMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okButton);
            okButton.Invoke();

            var adminWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Окно администратора")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(adminWindow);

            var addDoctorButton = adminWindow.FindFirstDescendant(cf => cf.ByAutomationId("AddDoctorButton")).AsButton();
            addDoctorButton.Invoke();
            Thread.Sleep(2000);

            var addDoctorWindow = _application.GetAllTopLevelWindows(_automation)
                                              .FirstOrDefault(w => w.Title.Contains("Добавить нового врача"));
            Assert.NotNull(addDoctorWindow);

            var firstNameTextBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var passwordBoxAdd = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("PasswordBox")).AsTextBox();
            var specializationComboBox = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("SpecializationComboBox")).AsComboBox();
            var roomComboBox = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("RoomNumberComboBox")).AsComboBox();
            var saveButton = addDoctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("SaveDoctorButton")).AsButton();

            lastNameTextBoxAdd.Text = "Иванов";
            passwordBoxAdd.Text = "qwerty123456!@";
            specializationComboBox.Select("Терапевт");
            roomComboBox.Select("Кабинет_101");

            saveButton.Invoke();

            Thread.Sleep(2000);

            var successMessageBoxAdd = _application.GetAllTopLevelWindows(_automation)
                                                 .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Пожалуйста, заполните все поля.")) != null);
            Assert.NotNull(successMessageBoxAdd);
        }

        public void Dispose()
        {
            _application.Close();
            _automation.Dispose();
        }
    }
}
