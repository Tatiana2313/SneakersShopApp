using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SneakersShopApp.Models;
using SneakersShopApp.Services;
using SneakersShopApp.Views;

namespace SneakersShopApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _login;

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _databaseService = new DatabaseService();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private bool CanExecuteLogin(object parameter) => !string.IsNullOrEmpty(Login);

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (string.IsNullOrEmpty(passwordBox?.Password))
            {
                MessageBox.Show("Пожалуйста, введите пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _databaseService.AuthenticateUser(Login, passwordBox.Password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Открываем главное окно с учетом роли пользователя
            var mainWindow = new MainWindow(user);
            mainWindow.Show();

            // Закрываем окно авторизации
            Application.Current.Windows[0].Close();

            /*Window windowToOpen = new Window();
            switch (user.NameRole)
            {
                case "admin":
                    windowToOpen = new MainWindow(user);
                    break;
                case "user":
                    //windowToOpen = new UserWindow(user);
                    break;
                case "seller":
                    //windowToOpen = new SellerWindow(user);
                    break;
                default:
                    throw new InvalidOperationException("Неизвестная роль пользователя");
            }

            windowToOpen.Show();
            Application.Current.Windows[0].Close();*/
        }
    }
}
