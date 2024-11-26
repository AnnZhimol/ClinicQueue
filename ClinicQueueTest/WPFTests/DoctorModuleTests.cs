using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.Core;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;

namespace ClinicQueueTest.WPFTests
{
    [Collection("Sequential")]
    public class DoctorModuleTests : IDisposable
    {
        private readonly Application _application;
        private readonly AutomationBase _automation;

        public DoctorModuleTests()
        {
            _automation = new UIA3Automation();
            _application = Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        [Fact, TestPriority(1)]
        public void TestDoctorStartAppointment_Success()
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
            var middleNameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("MiddleNameTextBox")).AsTextBox();
            var passwordBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("PasswordBox")).AsTextBox();
            var roleComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("RoleComboBox")).AsComboBox();
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("LoginButton")).AsButton();

            Assert.True(loginButton.IsEnabled, "Login button is not enabled.");
            roleComboBox.Select("Врач");

            firstNameTextBox.Text = "Николай";
            lastNameTextBox.Text = "Хижов";
            middleNameTextBox.Text = "Дмитриевич";
            passwordBox.Text = "qwerty12345#@";

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

            var doctorWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("DoctorWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(doctorWindow);
            var searchTextBox = doctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("SearchTextBox")).AsTextBox();
            var statusComboBox = doctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("StatusFilterComboBox")).AsComboBox();

            searchTextBox.Text = "Перова";
            statusComboBox.Select("Ожидание");

            Thread.Sleep(2000);

            var doctorsListView = doctorWindow.FindFirstDescendant(cf => cf.ByAutomationId("PatientsListView"));
            Assert.NotNull(doctorsListView);

            var doctorRows = doctorsListView.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataItem));

            var targetDoctorRow = doctorRows.FirstOrDefault(row =>
            {
                var cells = row.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                var surnameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Перова"));
                var nameCell = cells.FirstOrDefault(cell => cell.Name.Contains("Юлия"));
                var statusCell = cells.FirstOrDefault(cell => cell.Name.Contains("Ожидание"));
                return surnameCell != null && nameCell != null && statusCell != null;
            });

            Assert.NotNull(targetDoctorRow);

            targetDoctorRow.RightClick();

            Keyboard.Press(VirtualKeyShort.TAB);
            Keyboard.Press(VirtualKeyShort.ENTER);
        }

        public void Dispose()
        {
            _application.Close();
            _automation.Dispose();
        }
    }
}
