using System.Windows;

namespace HealthcareApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string documentNumber = DocumentNumberTextBox.Text.Trim();

            // Простейшая проверка (здесь можно подключить логику для проверки в БД)
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(documentNumber))
            {
                MessageTextBlock.Text = "Пожалуйста, заполните все поля.";
                return;
            }

            // Пример простой логики для проверки (замените на вашу логику валидации)
            if (fullName == "Иванов Иван" && documentNumber == "1234567890")
            {
                MessageTextBlock.Text = "Авторизация успешна!";
            }
            else
            {
                MessageTextBlock.Text = "Неверные данные. Попробуйте снова.";
            }
        }
    }
}
