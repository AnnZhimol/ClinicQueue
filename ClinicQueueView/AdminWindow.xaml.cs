﻿using ClinicQueueContracts.BindingModels;
using ClinicQueueContracts.BusinessLogicContracts;
using ClinicQueueContracts.SearchModels;
using ClinicQueueContracts.ViewModels;
using ClinicQueueDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class AdminWindow : Window
    {
        private readonly IAdminLogic _adminLogic;
        private readonly IDoctorLogic _doctorLogic;
        private List<DoctorViewModel> _allDoctors;
        private readonly AdminViewModel _admin;

        public AdminWindow(IAdminLogic adminLogic, IDoctorLogic doctorLogic, AdminViewModel admin)
        {
            _adminLogic = adminLogic;
            _doctorLogic = doctorLogic;
            _admin = admin;
            InitializeComponent();
            Loaded += AdminWindow_Loaded;
            SpecializationFilterComboBox.ItemsSource = Enum.GetValues(typeof(Specialization));
            LoadDoctors();
        }

        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WelcomeTextBox.Text = $"Добро пожаловать,\n{_admin.Surname} {_admin.Name} {_admin.Patronymic}";
        }

        private void LoadDoctors()
        {
            DoctorsListView.ItemsSource = _allDoctors = _doctorLogic.ReadList(null);
        }

        private void DoctorButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void QueueButton_Click(object sender, RoutedEventArgs e)
        {
            var queueWindow = new QueueWindow();
            queueWindow.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow(_adminLogic, _doctorLogic);
                mainWindow.Show();
                this.Close();
            }
        }

        private void UpdateDoctorsList()
        {
            var filteredDoctors = _allDoctors;

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                var searchSurname = SearchTextBox.Text.ToLower();
                filteredDoctors = filteredDoctors?
                    .Where(d => d.Surname.ToLower().Contains(searchSurname))
                    .ToList();
            }

            if (SpecializationFilterComboBox != null && SpecializationFilterComboBox.SelectedItem is Specialization selectedSpecialization)
            {
                if (((Specialization)SpecializationFilterComboBox.SelectedItem).ToString() != "Нет")
                {
                    filteredDoctors = filteredDoctors?
                        .Where(d => d.Specialization == selectedSpecialization.ToString())
                        .ToList();
                }
                else
                {
                    filteredDoctors = filteredDoctors?
                        .Where(d => d.Specialization != null)
                        .ToList();
                }
            }

            if (DoctorsListView != null)
            {
                DoctorsListView.ItemsSource = filteredDoctors;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDoctorsList();
        }

        private void SpecializationFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDoctorsList();
        }

        private void DoctorsListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DoctorsListView.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите врача для выполнения действия.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteDoctorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorsListView.SelectedItem is DoctorViewModel selectedDoctor)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить врача {selectedDoctor.Surname}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _doctorLogic.Delete(ConvertToBindingModel(selectedDoctor));
                    MessageBox.Show("Врач успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDoctors();
                }

            }
        }
        private DoctorBindingModel ConvertToBindingModel(DoctorViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            return new DoctorBindingModel
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Surname = viewModel.Surname,
                Patronymic = viewModel.Patronymic,
                Password = viewModel.Password,
                Specialization = viewModel.Specialization,
                CabinetNumber = viewModel.CabinetNumber
            };
        }

        private void EditDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorsListView.SelectedItem is DoctorViewModel selectedDoctor)
            {
                var editDoctorWindow = new EditDoctorWindow(_doctorLogic, selectedDoctor);
                editDoctorWindow.ShowDialog();
                LoadDoctors();
            }
        }

        private void AddDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            var addDoctorWindow = new AddDoctorWindow(_doctorLogic);
            addDoctorWindow.ShowDialog();
            LoadDoctors();
        }
    }
}