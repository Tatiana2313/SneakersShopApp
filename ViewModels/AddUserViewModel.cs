using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace SneakersShopApp.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private string _roleName;

        public string LoginUser
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(LoginUser));
            }
        }

        public string PasswordUser
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(PasswordUser));
            }
        }

        public string NameRole
        {
            get => _roleName;
            set
            {
                _roleName = value;
                OnPropertyChanged(nameof(NameRole));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action<bool> CloseAction { get; set; }

        public AddUserViewModel()
        {
            SaveCommand = new RelayCommand(o => Add());
            CancelCommand = new RelayCommand(o => Cancel());
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(PasswordUser))
            {
                MessageBox.Show("Пароль обязателен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordUser.Length != 8)
            {
                MessageBox.Show("Пароль должен содержать ровно 8 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CloseAction?.Invoke(true);
        }

        private void Cancel()
        {
            CloseAction?.Invoke(false);
        }
    }
}
