using SneakersShopApp.Models;
using SneakersShopApp.Services;
using SneakersShopApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SneakersShopApp.ViewModels
{
    public class ProviderViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Provider> _providers;
        private Provider _selectedProvider;
        private string _searchTerm;

        public ObservableCollection<Provider> Providers
        {
            get => _providers;
            set { _providers = value; OnPropertyChanged(); }
        }

        public Provider SelectedProvider
        {
            get => _selectedProvider;
            set { _selectedProvider = value; OnPropertyChanged(); }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                SearchProviders();
            }
        }

        public ICommand AddProviderCommand { get; private set; }
        public ICommand EditProviderCommand { get; private set; }
        public ICommand DeleteProviderCommand { get; private set; }

        public ProviderViewModel()
        {
            _databaseService = new DatabaseService();

            AddProviderCommand = new RelayCommand(AddProvider);
            EditProviderCommand = new RelayCommand(EditProvider, CanEditOrDeleteProvider);
            DeleteProviderCommand = new RelayCommand(DeleteProvider, CanEditOrDeleteProvider);

            LoadProviders();
        }

        private void LoadProviders()
        {
            try
            {
                Providers = new ObservableCollection<Provider>(_databaseService.GetProviders());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void SearchProviders()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                LoadProviders();
            }
            else
            {
                var filteredList = Providers.Where(s =>
                    s.ProviderName.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Addres.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                Providers = new ObservableCollection<Provider>(filteredList);
            }
        }

        private void AddProvider(object parameter)
        {
            var provider = new Provider();
            var window = new AddEditProviderWindow(provider, true)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                LoadProviders();
                MessageBox.Show("Поставщик успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditProvider(object parameter)
        {
            if (SelectedProvider != null)
            {
                var providerCopy = new Provider
                {
                    CodProdiver = SelectedProvider.CodProdiver,
                    ProviderName = SelectedProvider.ProviderName,
                    Addres = SelectedProvider.Addres,
                    PhoneFax = SelectedProvider.PhoneFax,
                    RascetScet = SelectedProvider.RascetScet
                };
                var editWindow = new AddEditProviderWindow(providerCopy, false)
                {
                    Owner = Application.Current.MainWindow
                };
                if (editWindow.ShowDialog() == true)
                {
                    LoadProviders();
                    MessageBox.Show("Поставщик успешно отредактирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteProvider(object parameter)
        {
            if (SelectedProvider != null && MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{SelectedProvider.ProviderName}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteProvider(SelectedProvider.CodProdiver);
                    MessageBox.Show($"Поставщик '{SelectedProvider.ProviderName}' успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProviders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении поставщика: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDeleteProvider(object parameter)
        {
            return SelectedProvider != null;
        }
    }
}
