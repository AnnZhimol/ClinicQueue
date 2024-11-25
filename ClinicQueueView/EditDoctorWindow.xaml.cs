using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ClinicQueueView
{
    public partial class EditDoctorWindow : Window
    {
        private readonly DoctorViewModel _doctor;
        private readonly IDoctorLogic _doctorLogic;

        public EditDoctorWindow(IDoctorLogic doctorLogic, DoctorViewModel doctor)
        {
            InitializeComponent();
            _doctorLogic = doctorLogic;
            _doctor = doctor;
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(Specialization));
            RoomNumberComboBox.ItemsSource = Enum.GetValues(typeof(RoomNumber));
            LoadDoctorData();
        }

        private void LoadDoctorData()
        {
            FirstNameTextBox.Text = _doctor.Name;
            LastNameTextBox.Text = _doctor.Surname;
            MiddleNameTextBox.Text = _doctor.Patronymic;
            SpecializationComboBox.SelectedItem = Enum.Parse(typeof(Specialization), _doctor.Specialization.ToString());
            RoomNumberComboBox.SelectedItem = Enum.Parse(typeof(RoomNumber), _doctor.CabinetNumber.ToString());
            PasswordBox.Password = _doctor.Password;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите изменить данные?", "Подтверждение изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (!Regex.IsMatch(PasswordBox.Password, @"^^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Пароль должен содержать латинские буквы, цифры и спец. символы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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
            if (string.IsNullOrEmpty(FirstNameTextBox.Text) || string.IsNullOrEmpty(LastNameTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password) || ((RoomNumber)RoomNumberComboBox.SelectedItem).ToString() == "Нет" || ((Specialization)SpecializationComboBox.SelectedItem).ToString() == "Нет")
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string? middleName = MiddleNameTextBox.Text;

            if (string.IsNullOrEmpty(middleName) || middleName == "Введите отчество")
            {
                middleName = null;
            }

            if (result == MessageBoxResult.Yes)
            {
                var updatedDoctor = new DoctorBindingModel
                {
                    Id = _doctor.Id,
                    Name = FirstNameTextBox.Text,
                    Surname = LastNameTextBox.Text,
                    Patronymic = middleName,
                    Password = PasswordBox.Password,
                    Specialization = ((Specialization)SpecializationComboBox.SelectedItem).ToString(),
                    CabinetNumber = ((RoomNumber)RoomNumberComboBox.SelectedItem).ToString()
                };

                _doctorLogic.Update(updatedDoctor);

                MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
        }
    }
}
