using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SneakersShopApp.Models;
using SneakersShopApp.Services;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace SneakersShopApp.ViewModels
{
    public class EditSneakersViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private Sneaker _sneaker;
        private ObservableCollection<KindSport> _kindSports;

        public Sneaker Sneaker
        {
            get => _sneaker;
            set
            {
                _sneaker = value;
                OnPropertyChanged(nameof(Sneaker));
            }
        }

        public ObservableCollection<KindSport> KindSport
        {
            get => _kindSports;
            set
            {
                _kindSports = value;
                OnPropertyChanged(nameof(KindSport));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowsePhotoCommand { get; }

        public EditSneakersViewModel(Sneaker sneakers)
        {
            _databaseService = new DatabaseService();
            _sneaker = sneakers;

            LoadComboBoxData();
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            BrowsePhotoCommand = new RelayCommand(BrowsePhoto);
        }

        private void LoadComboBoxData()
        {
            KindSport = new ObservableCollection<KindSport>(_databaseService.GetKindOfSport());
            if (_sneaker.KindOfSport != null && _sneaker.KindOfSport.CodKindSport != 0)
            {
                var selectedSport = KindSport.FirstOrDefault(f => f.CodKindSport == _sneaker.KindOfSport.CodKindSport);
                if (selectedSport != null) _sneaker.KindOfSport = selectedSport;
                else _sneaker.KindOfSport = null;
            }
            else _sneaker.KindOfSport = null;
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Sneaker.SneakersName) && Sneaker.UnitPrice > 0;
        }

        private void Save(object parameter)
        {
            try
            {
                _databaseService.UpdateSneakers(Sneaker);

                MessageBox.Show($"Кроссовки успешно отредактированы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                var window = parameter as Window;
                if (window != null)
                    window.DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка при редактировании информации о кроссовках", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            var window = parameter as Window;
            if (window != null)
                window.DialogResult = false;
        }

        private void BrowsePhoto(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
                Title = "Выберите изображение кроссовок"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFilePath);

                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagesPath = Path.Combine(projectDirectory, "Images");

                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    string destinationFilePath = Path.Combine(imagesPath, fileName);

                    string runtimeImagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!Directory.Exists(runtimeImagesPath))
                    {
                        Directory.CreateDirectory(runtimeImagesPath);
                    }

                    string runtimeDestinationPath = Path.Combine(runtimeImagesPath, fileName);

                    File.Copy(sourceFilePath, destinationFilePath, true);
                    File.Copy(sourceFilePath, runtimeDestinationPath, true);

                    Sneaker.Photo = fileName;
                    OnPropertyChanged(nameof(Sneaker));

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
