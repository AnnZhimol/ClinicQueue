using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueDataBaseImplement.Models;
using ClinicQueueDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClinicQueueView
{
    public partial class AddDoctorWindow : Window
    {
        private readonly IDoctorLogic _doctorLogic;
        public AddDoctorWindow(IDoctorLogic doctorLogic)
        {
            _doctorLogic = doctorLogic;
            InitializeComponent();
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(Specialization));
            RoomNumberComboBox.ItemsSource = Enum.GetValues(typeof(RoomNumber));
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

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Tag.ToString() == "Введите пароль")
                {
                    passwordBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    passwordBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string? middleName = MiddleNameTextBox.Text;
            if (!Regex.IsMatch(PasswordBox.Password, @"^^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Пароль должен содержать латинские буквы, цифры и спец. символы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string password = PasswordBox.Password;
            if (RoomNumberComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите кабинет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SpecializationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите специализацию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string? cabinet = ((RoomNumber)RoomNumberComboBox.SelectedItem).ToString();
            string? specialization = ((Specialization)SpecializationComboBox.SelectedItem).ToString();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password) || cabinet == "Нет" || specialization == "Нет")
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(middleName) || middleName == "Введите отчество")
            {
                middleName = null;
            }

            var newDoctor = new DoctorBindingModel
            {
                Name = firstName,
                Surname = lastName,
                Patronymic = middleName,
                Password = password,
                Specialization = specialization,
                CabinetNumber = cabinet
            };

            _doctorLogic.Create(newDoctor);
            MessageBox.Show("Врач успешно добавлен!");
            Close();
        }
    }
}
