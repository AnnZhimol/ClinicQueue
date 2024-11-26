using FlaUI.Core.AutomationElements;
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
using FlaUI.Core.Input;

namespace ClinicQueueTest.WPFTests
{
    [Collection("Sequential")]
    public class ScheduleTests : IDisposable
    {
        private readonly FlaUI.Core.Application _application;
        private readonly AutomationBase _automation;

        public ScheduleTests()
        {
            _automation = new UIA3Automation();
            _application = FlaUI.Core.Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        public void Dispose()
        {
            _application.Close();
            _automation.Dispose();
        }

        [Fact, TestPriority(2)]
        public void TestScheduleUpdate_Success()
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
            Keyboard.Press(VirtualKeyShort.ENTER);

            var scheduleWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("Редактирование расписания врача")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(scheduleWindow);

            var daysOfWeekPanel = scheduleWindow.FindFirstDescendant(cf => cf.ByAutomationId("DaysOfWeekControl"));
            Assert.NotNull(daysOfWeekPanel);

            var random = new Random();
            var checkBoxes = daysOfWeekPanel.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.CheckBox));

            Assert.True(checkBoxes.Length > 0, "No checkboxes found in the schedule window.");

            foreach (var checkBox in checkBoxes)
            {
                if (random.Next(2) == 0)
                {
                    checkBox.AsCheckBox().IsChecked = true;
                }
                if (random.Next(2) == 0)
                {
                    checkBox.AsCheckBox().IsChecked = false;
                }
            }

            var saveButton = scheduleWindow.FindFirstDescendant(cf => cf.ByAutomationId("SaveScheduleButton")).AsButton();
            Assert.NotNull(saveButton);
            saveButton.Invoke();

            var successSaveMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Расписание успешно сохранено.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successSaveMessageBox);

            var okSaveButton = successSaveMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okSaveButton);
            okSaveButton.Invoke();
        }
    }
}
