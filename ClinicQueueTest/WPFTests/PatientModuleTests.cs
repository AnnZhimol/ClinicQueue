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
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace ClinicQueueTest.WPFTests
{
    [Collection("Sequential")]
    public class PatientModuleTests : IDisposable
    {
        private readonly Application _application;
        private readonly AutomationBase _automation;

        AutomationElement FindFirstAvailableDate(Calendar calendar)
        {
            var calendarButtons = calendar.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));

            foreach (var button in calendarButtons)
            {
                button.DoubleClick();
                Thread.Sleep(500);

                var dayItems = calendar.FindAllDescendants(cf => cf.ByControlType(ControlType.ListItem));

                foreach (var dayItem in dayItems)
                {
                    if (dayItem.Properties.IsEnabled)
                    {
                        return dayItem; 
                    }
                }
            }

            return null;
        }

        public PatientModuleTests()
        {
            _automation = new UIA3Automation();
            _application = Application.Launch("C:\\UlSTU\\KPO\\ClinicQueue\\ClinicQueueView\\bin\\Debug\\net8.0-windows\\ClinicQueueView.exe");
        }

        [Fact, TestPriority(1)]
        public void TestPatientLogin_Success()
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

            var reserveButton = patientMainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ReserveButton")).AsButton();

            reserveButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER); 

            var reserveWindow = Retry.WhileNull(
                () => _application.GetAllTopLevelWindows(_automation)
                                  .FirstOrDefault(w => w.Title.Contains("ReserveWindow")),
                TimeSpan.FromSeconds(5)
            ).Result;
            Assert.NotNull(reserveWindow);

            var specializationComboBox = reserveWindow.FindFirstDescendant(cf => cf.ByAutomationId("SpecializationComboBox")).AsComboBox();
            var doctorComboBox = reserveWindow.FindFirstDescendant(cf => cf.ByAutomationId("DoctorComboBox")).AsComboBox();
            var calendar = reserveWindow.FindFirstDescendant(cf => cf.ByAutomationId("AppointmentCalendar")).AsCalendar();
            var timeSlotComboBox = reserveWindow.FindFirstDescendant(cf => cf.ByAutomationId("TimeSlotComboBox")).AsComboBox();
            var bookAppointmentButton = reserveWindow.FindFirstDescendant(cf => cf.ByAutomationId("ReserveButton")).AsButton();

            specializationComboBox.Select("Дерматолог");
            doctorComboBox.Select(0);

            var availableDate = FindFirstAvailableDate(calendar);

            timeSlotComboBox.Select(0);
            Keyboard.Press(VirtualKeyShort.TAB);

            bookAppointmentButton.Focus();
            Keyboard.Press(VirtualKeyShort.ENTER);

            Thread.Sleep(2000);

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
