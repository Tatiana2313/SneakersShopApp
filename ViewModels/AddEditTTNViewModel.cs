using SneakersShopApp.Models;
using SneakersShopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace SneakersShopApp.ViewModels
{
    public class AddEditTTNViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly bool _isAddMode;
        private TTN _ttn;
        private ObservableCollection<Provider> _providers;
        private ObservableCollection<Sneaker> _sneakers;

        public TTN TTN
        {
            get => _ttn;
            set
            {
                _ttn = value;
                OnPropertyChanged(nameof(TTN));
            }
        }

        public bool IsAddMode
        {
            get => _isAddMode;
        }

        public string WindowTitle
        {
            get => $"{(IsAddMode ? "Добавление" : "Редактирование")} накладной";
        }

        public ObservableCollection<Provider> Providers
        {
            get => _providers;
            set
            {
                _providers = value;
                OnPropertyChanged(nameof(Providers));
            }
        }

        public ObservableCollection<Sneaker> Sneakers
        {
            get => _sneakers;
            set
            {
                _sneakers = value;
                OnPropertyChanged(nameof(Sneakers));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddEditTTNViewModel(TTN ttn, bool isAddMode)
        {
            _databaseService = new DatabaseService();
            _isAddMode = isAddMode;
            _ttn = ttn ?? new TTN
            {
                Provider = new Provider(),
                Sneakers = new Sneaker(),
                DatePost = DateTime.Now
            };

            LoadComboBoxData();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadComboBoxData()
        {
            try
            {
                Providers = new ObservableCollection<Provider>(_databaseService.GetProviders());
                if (_ttn.Provider != null && _ttn.Provider.CodProdiver != 0)
                {
                    var selectedProvider = Providers.FirstOrDefault(p => p.CodProdiver == _ttn.Provider.CodProdiver);
                    if (selectedProvider != null) _ttn.Provider = selectedProvider;
                    else _ttn.Provider = null;
                }
                else _ttn.Provider = null;

                Sneakers = new ObservableCollection<Sneaker>(_databaseService.GetSneakers());
                if (_ttn.Sneakers != null && _ttn.Sneakers.CodSneakers != 0)
                {
                    var selectedSneakers = Sneakers.FirstOrDefault(a => a.CodSneakers == _ttn.Sneakers.CodSneakers);
                    if (selectedSneakers != null) _ttn.Sneakers = selectedSneakers;
                    else _ttn.Sneakers = null;
                }
                else _ttn.Sneakers = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save(object parameter)
        {
            try
            {
                if (_isAddMode) _databaseService.AddTTN(TTN);
                else _databaseService.UpdateTTN(TTN);

                var window = parameter as Window;
                if (window != null)
                    window.DialogResult = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении накладной: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            var window = parameter as Window;
            if (window != null)
                window.DialogResult = false;
        }
    }
}
