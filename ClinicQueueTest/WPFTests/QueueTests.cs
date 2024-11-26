using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.Core;
using FlaUI.UIA3;
using Xunit;
using FlaUI.Core.Input;
using FlaUI.Core.Definitions;

namespace ClinicQueueTest.WPFTests
{
    [Collection("Sequential")]
    public class QueueTests : IDisposable
    {
        private readonly FlaUI.Core.Application _application;
        private readonly AutomationBase _automation;

        public QueueTests()
        {
            _automation = new UIA3Automation();
            _application = FlaUI.Core.Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        private void SelectDate(AutomationElement datePicker, DateTime date)
        {
            var textBox = datePicker.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit))?.AsTextBox();
            if (textBox != null)
            {
                textBox.Text = date.ToShortDateString(); 
                return;
            }

            datePicker.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);
            Thread.Sleep(500);
        }

        public void Dispose()
        {
            _application.Close();
            _automation.Dispose();
        }

        [Fact, TestPriority(2)]
        public void TestElectronicQueueAdd_Success()
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

            var doctorsListView = adminWindow.FindFirstDescendant(cf => cf.ByAutomationId("DoctorsListView"));
            Assert.NotNull(doctorsListView);

            var doctorRows = doctorsListView.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataItem));

            var targetDoctorRow = doctorRows.FirstOrDefault(row =>
            {
                var cells = row.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                var surnameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Брескану"));
                var nameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Никита"));
                return surnameCell != null && nameCell != null;
            });

            Assert.NotNull(targetDoctorRow);

            targetDoctorRow.RightClick();

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var queueWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Управление электронными очередями")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(queueWindow);

            var queueNameTextBox = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("QueueNameTextBox")).AsTextBox();
            var startDatePicker = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("StartDatePicker"));
            var endDatePicker = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("EndDatePicker"));
            var createButton = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("CreateQueueButton")).AsButton();

            queueNameTextBox.Text = "Test Queue";

            SelectDate(startDatePicker, DateTime.Now.AddDays(1));
            SelectDate(endDatePicker, DateTime.Now.AddDays(7));

            createButton.Invoke();

            var successMessageBoxQueue = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Новая очередь создана и активирована.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBoxQueue);
        }

        [Fact, TestPriority(2)]
        public void TestElectronicQueueComplete_Success()
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

            var doctorsListView = adminWindow.FindFirstDescendant(cf => cf.ByAutomationId("DoctorsListView"));
            Assert.NotNull(doctorsListView);

            var doctorRows = doctorsListView.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataItem));

            var targetDoctorRow = doctorRows.FirstOrDefault(row =>
            {
                var cells = row.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                var surnameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Брескану"));
                var nameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Никита"));
                return surnameCell != null && nameCell != null;
            });

            Assert.NotNull(targetDoctorRow);

            targetDoctorRow.RightClick();

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var queueWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Управление электронными очередями")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(queueWindow);

            var listView = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("QueueDataGrid"));
            Assert.NotNull(doctorsListView);

            var rows = listView.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataItem));

            var targetRow = rows.FirstOrDefault(row =>
            {
                var cells = row.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                var surnameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Активна"));
                var nameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Test Queue"));
                return surnameCell != null && nameCell != null;
            });

            Assert.NotNull(targetRow);

            targetRow.RightClick();

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var successMessageBoxComplete = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Вы уверены, что хотите завершить очередь Test Queue?")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBoxComplete);

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var successMessageBoxQueue = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Очередь завершена.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBoxQueue);
        }

        [Fact, TestPriority(2)]
        public void TestElectronicQueueAdd_Error()
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

            var doctorsListView = adminWindow.FindFirstDescendant(cf => cf.ByAutomationId("DoctorsListView"));
            Assert.NotNull(doctorsListView);

            var doctorRows = doctorsListView.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataItem));

            var targetDoctorRow = doctorRows.FirstOrDefault(row =>
            {
                var cells = row.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                var surnameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Брескану"));
                var nameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Никита"));
                return surnameCell != null && nameCell != null;
            });

            Assert.NotNull(targetDoctorRow);

            targetDoctorRow.RightClick();

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var queueWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Управление электронными очередями")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(queueWindow);

            var queueNameTextBox = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("QueueNameTextBox")).AsTextBox();
            var startDatePicker = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("StartDatePicker"));
            var endDatePicker = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("EndDatePicker"));
            var createButton = queueWindow.FindFirstDescendant(cf => cf.ByAutomationId("CreateQueueButton")).AsButton();

            queueNameTextBox.Text = "Test Queue";

            SelectDate(startDatePicker, DateTime.Now.AddDays(7));
            SelectDate(endDatePicker, DateTime.Now.AddDays(1));

            createButton.Invoke();

            var successMessageBoxQueue = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Дата окончания должна быть позже даты начала.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successMessageBoxQueue);
        }
    }
}
