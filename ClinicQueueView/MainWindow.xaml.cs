using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClinicQueueView
{
    public partial class MainWindow : Window
    {
        private readonly IAdminLogic _adminLogic;
        private readonly IDoctorLogic _doctorLogic;
        public MainWindow(IAdminLogic adminLogic, IDoctorLogic doctorLogic)
        {
            _adminLogic = adminLogic;
            _doctorLogic = doctorLogic;
            InitializeComponent();
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string? middleName = MiddleNameTextBox.Text;
            string password = PasswordBox.Password;
            string? role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password) || role == "Выберите роль")
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(middleName) || middleName == "Введите отчество")
            {
                middleName = null;
            }

            if (role == "Администратор")
            {
                AdminViewModel? admin = _adminLogic.ReadElement(new AdminSearchModel
                {
                    Name = firstName,
                    Surname = lastName,
                    Patronymic = middleName,
                    Password = password
                });

                if (admin != null)
                {
                    MessageBox.Show("Добро пожаловать, Администратор!", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                    AdminWindow adminWindow = new AdminWindow(_adminLogic, _doctorLogic, admin);
                    adminWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильное имя, фамилия, отчество или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (role == "Врач")
            {
                DoctorViewModel? doctor = _doctorLogic.ReadElement(new DoctorSearchModel
                {
                    Name = firstName,
                    Surname = lastName,
                    Patronymic = middleName,
                    Password = password
                });

                if (doctor != null)
                {
                    MessageBox.Show("Добро пожаловать, Врач!", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
  
                    //DoctorWindow doctorWindow = new DoctorWindow(); // Замените DoctorWindow на ваше окно врача
                    //doctorWindow.Show();
                    //Close();
                }
                else
                {
                    MessageBox.Show("Неправильное имя, фамилия, отчество или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
