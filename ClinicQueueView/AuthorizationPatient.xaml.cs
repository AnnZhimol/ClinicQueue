using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataBaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClinicQueueView
{
    public partial class AuthorizationPatient : Window
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IAppointmentLogic _appointmentLogic;
        private readonly IDoctorLogic _doctorLogic;

        public AuthorizationPatient(IPatientLogic patientLogic, IAppointmentLogic appointmentLogic, IDoctorLogic doctorLogic)
        {
            _patientLogic = patientLogic;
            InitializeComponent();
            _appointmentLogic = appointmentLogic;
            _doctorLogic = doctorLogic;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == textBox.Tag.ToString())
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = textBox.Tag.ToString();
                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            PatientWindow patientWindow = new PatientWindow(_patientLogic, _doctorLogic, _appointmentLogic);
            patientWindow.Show();
            this.Close();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string? middleName = MiddleNameTextBox.Text;
            string? documentType = (DocumentTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string documentNumber = DocumentNumberTextBox.Text;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(documentNumber) || documentType == "Выберите тип документа")
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите тип документа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(middleName) || middleName == "Введите отчество")
            {
                middleName = null;
            }

            if (documentType == "ОМС")
            {
                PatientViewModel? patient = _patientLogic.ReadElement(new PatientSearchModel
                {
                    Name = firstName,
                    Surname = lastName,
                    Patronymic = middleName,
                    OMSNumber = documentNumber
                });

                if (patient != null)
                {
                    PatientMainWindow patientMainWindow = new PatientMainWindow(_patientLogic, patient, _doctorLogic, _appointmentLogic);
                    patientMainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильное имя, фамилия, отчество или номер документа.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (documentType == "Паспорт РФ")
            {
                PatientViewModel? patient = _patientLogic.ReadElement(new PatientSearchModel
                {
                    Name = firstName,
                    Surname = lastName,
                    Patronymic = middleName,
                    PassportNumber = documentNumber
                });

                if (patient != null)
                {
                    PatientMainWindow patientMainWindow = new PatientMainWindow(_patientLogic, patient, _doctorLogic, _appointmentLogic);
                    patientMainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильное имя, фамилия, отчество или номер документа.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
