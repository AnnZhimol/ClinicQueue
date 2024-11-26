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
    public class PatientAuthTests : IDisposable
    {
        private readonly Application _application;
        private readonly AutomationBase _automation;

        public PatientAuthTests()
        {
            _automation = new UIA3Automation();
            _application = Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        [Fact, TestPriority(1)]
        public void TestDoctorLogin_Success()
        {
            var startWindow = _application.GetMainWindow(_automation);

            var employeeButton = startWindow.FindFirstDescendant(cf => cf.ByAutomationId("PatientButton")).AsButton();

            employeeButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successStartMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Запуск приложения для пациента.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successStartMessageBox);

            var okStartButton = successStartMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okStartButton);
            okStartButton.Invoke();

            var patientWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("PatientWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(patientWindow);

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var authWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("AuthorizationPatient")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(authWindow);

            var firstNameTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var docComboBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("DocumentTypeComboBox")).AsComboBox();
            var docNumberTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("DocumentNumberTextBox")).AsTextBox();
            var entryButton = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("EntryButton")).AsButton();

            Assert.True(entryButton.IsEnabled, "Login button is not enabled.");
            docComboBox.Select("Паспорт РФ");

            firstNameTextBox.Text = "Юлия";
            lastNameTextBox.Text = "Перова";
            docNumberTextBox.Text = "865973";

            entryButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var patientMainWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("PatientMainWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(patientMainWindow);
        }

        [Fact, TestPriority(1)]
        public void TestDoctorLogin_Error()
        {
            var startWindow = _application.GetMainWindow(_automation);

            var employeeButton = startWindow.FindFirstDescendant(cf => cf.ByAutomationId("PatientButton")).AsButton();

            employeeButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var successStartMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Запуск приложения для пациента.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(successStartMessageBox);

            var okStartButton = successStartMessageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button))?.AsButton();
            Assert.NotNull(okStartButton);
            okStartButton.Invoke();

            var patientWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("PatientWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(patientWindow);

            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.TAB);
            Thread.Sleep(100);
            Keyboard.Press(VirtualKeyShort.ENTER);

            var authWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("AuthorizationPatient")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(authWindow);

            var firstNameTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("FirstNameTextBox")).AsTextBox();
            var lastNameTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("LastNameTextBox")).AsTextBox();
            var docComboBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("DocumentTypeComboBox")).AsComboBox();
            var docNumberTextBox = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("DocumentNumberTextBox")).AsTextBox();
            var entryButton = authWindow.FindFirstDescendant(cf => cf.ByAutomationId("EntryButton")).AsButton();

            Assert.True(entryButton.IsEnabled, "Login button is not enabled.");
            docComboBox.Select("Паспорт РФ");

            firstNameTextBox.Text = "WrongName";
            lastNameTextBox.Text = "WrongSurname";
            docNumberTextBox.Text = "123456";

            entryButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

            var errorStartMessageBox = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.FindFirstDescendant(cf => cf.ByText("Неправильное имя, фамилия, отчество или номер документа.")) != null),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(errorStartMessageBox);
        }

        public void Dispose()
        {
            _application.Close();
            _automation.Dispose();
        }
    }
}
