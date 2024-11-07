using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
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

        private void SaveDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            var updatedDoctor = new DoctorBindingModel
            {
                Id = _doctor.Id,
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Patronymic = MiddleNameTextBox.Text,
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
