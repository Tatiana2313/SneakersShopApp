using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SneakersShopApp.ViewModels;

namespace SneakersShopApp.Views
{
    public partial class AddUserWindow : Window
    {
        private readonly AddUserViewModel viewModel;
        public AddUserViewModel ViewModel => viewModel;

        public AddUserWindow()
        {
            InitializeComponent();

            viewModel = new AddUserViewModel();
            DataContext = viewModel;

            viewModel.CloseAction = result =>
            {
                DialogResult = result;
                Close();
            };

            var passwordBox = this.FindName("PasswordBox") as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged += (s, e) =>
                {
                    viewModel.PasswordUser = passwordBox.Password;
                };
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoleComboBox.SelectedItem == null || LoginBox.Text == null || PasswordBox == null) 
            {
                MessageBox.Show($"Заполните все поля!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
                {
                    return;
                }

                if (passwordBox.Password.Length >= 8)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
