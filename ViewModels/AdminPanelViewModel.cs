using SneakersShopApp.Models;
using SneakersShopApp.Services;
using SneakersShopApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace SneakersShopApp.ViewModels
{
    public class AdminPanelViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<User> _users;
        private bool _isEditDialogOpen;
        private User _selectedUser;

        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set
            {
                _isEditDialogOpen = value;
                OnPropertyChanged(nameof(IsEditDialogOpen));
            }
        }

        public ICommand AddUserCommand { get; private set; }
        public ICommand EditUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand CreateBackupCommand { get; private set; }
        public ICommand RestoreBackupCommand { get; private set; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelEditCommand { get; }

        public AdminPanelViewModel()
        {
            _databaseService = new DatabaseService();

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
            CreateBackupCommand = new RelayCommand(CreateBackup);
            RestoreBackupCommand = new RelayCommand(RestoreBackup);
            SaveUserCommand = new RelayCommand(SaveUser);
            CancelEditCommand = new RelayCommand(CancelEdit);

            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                Users = new ObservableCollection<User>(_databaseService.GetUsers());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddUser(object parameter)
        {
            var addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true)
            {
                var viewModel = addUserWindow.ViewModel;
                try
                {
                    var existingUser = Users.FirstOrDefault(u => u.LoginUser == viewModel.LoginUser);
                    if (existingUser != null)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var newUser = new User
                    {
                        LoginUser = viewModel.LoginUser,
                        NameRole = viewModel.NameRole,
                        PasswordUser = string.IsNullOrWhiteSpace(viewModel.PasswordUser)
                            ? BCrypt.Net.BCrypt.HashPassword("default123")
                            : BCrypt.Net.BCrypt.HashPassword(viewModel.PasswordUser)
                    };

                    _databaseService.AddUser(newUser);

                    LoadUsers();
                    LogAction($"Added user: {newUser.LoginUser}");
                    MessageBox.Show($"Пользователь {newUser.LoginUser} добавлен. Пароль: {viewModel.PasswordUser}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditUser(object parameter)
        {

            if (parameter is User user)
            {
                SelectedUser = new User
                {
                    IdUser = user.IdUser,
                    LoginUser = user.LoginUser,
                    NameRole = user.NameRole,
                    PasswordUser = user.PasswordUser
                };
                IsEditDialogOpen = true;
            }
        }

        private void DeleteUser(object parameter)
        {
            if (parameter is User user && MessageBox.Show($"Вы уверены, что хотите удалить пользователя '{user.LoginUser}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteUser(user.IdUser);
                    MessageBox.Show($"Пользователь '{user.LoginUser}' успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Users.Remove(user);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResetPassword(object parameter)
        {
            if (parameter is User user)
            {
                try
                {
                    string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword);
                    user.PasswordUser = hashedPassword;
                    _databaseService.UpdateUser(user);
                    LogAction($"Reset password for user: {user.LoginUser}");
                    MessageBox.Show($"Пароль для {user.LoginUser} сброшен. Временный пароль: {tempPassword}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сброса пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateBackup(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                Title = "Выберите место для сохранения резервной копии",
                DefaultExt = "bak"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _databaseService.CreateBackupDB(saveFileDialog.FileName);
                    LogAction($"Created backup: {saveFileDialog.FileName}");
                    MessageBox.Show($"Резервная копия успешно создана: {saveFileDialog.FileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания резервной копии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RestoreBackup(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Backup Files (*.bak)|*.bak",
                Title = "Выберите файл резервной копии для восстановления"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _databaseService.RestoreBackupDB(openFileDialog.FileName);
                    LogAction($"Restored backup: {openFileDialog.FileName}");
                    MessageBox.Show($"Данные успешно восстановлены из: {openFileDialog.FileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка восстановления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LogAction(string action)
        {
            try
            {
                File.AppendAllText("admin_log.txt", $"{DateTime.Now}: {action}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging action: {ex.Message}");
            }
        }

        private void SaveUser(object parameter)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Пользователь не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUser.LoginUser) || string.IsNullOrWhiteSpace(SelectedUser.NameRole))
            {
                MessageBox.Show("Имя пользователя и роль обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingUser = Users.FirstOrDefault(u => u.LoginUser == SelectedUser.LoginUser && u.IdUser != SelectedUser.IdUser);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var userToUpdate = Users.FirstOrDefault(u => u.IdUser == SelectedUser.IdUser);
                if (userToUpdate != null)
                {
                    userToUpdate.LoginUser = SelectedUser.LoginUser;
                    userToUpdate.NameRole = SelectedUser.NameRole;
                    _databaseService.UpdateUser(userToUpdate);
                    LogAction($"Updated user: {SelectedUser.LoginUser}");
                    MessageBox.Show($"Пользователь {SelectedUser.LoginUser} обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers();
                }

                IsEditDialogOpen = false;
                SelectedUser = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelEdit(object parameter)
        {
            IsEditDialogOpen = false;
            SelectedUser = null;
        }
    }
}
