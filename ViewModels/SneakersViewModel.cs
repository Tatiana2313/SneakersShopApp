using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SneakersShopApp.Models;
using SneakersShopApp.Views;
using SneakersShopApp.Services;
using System.Windows.Controls;
using System.Linq;

namespace SneakersShopApp.ViewModels
{
    public class SneakersViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly User _user;
        private ObservableCollection<Sneaker> _sneakers;
        private Sneaker _selectedSneaker;
        private string _searchTerm;
        private ObservableCollection<Sneaker> _originalSneakers;
        private int _totalCount;
        private FilterSneakersViewModel _currentFilter;
        private FilterSneakersWindow _filterWindow;

        public ObservableCollection<Sneaker> Sneakers
        {
            get => _sneakers;
            set { _sneakers = value; OnPropertyChanged(); }
        }

        public Sneaker SelectedSneaker
        {
            get => _selectedSneaker;
            set { _selectedSneaker = value; OnPropertyChanged(); }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                SearchSneakers();
            }
        }

        public int TotalCount
        {
            get => _totalCount;
            set { _totalCount = value; OnPropertyChanged(); }
        }

        public bool IsAdmin => _user.NameRole == "admin";

        public ICommand AddSneakersCommand { get; private set; }
        public ICommand EditSneakersCommand { get; private set; }
        public ICommand DeleteSneakersCommand { get; private set; }
        public ICommand ToggleFiltersCommand { get; private set; }

        public SneakersViewModel(User user)
        {
            _user = user;
            _databaseService = new DatabaseService();

            AddSneakersCommand = new RelayCommand(ExecuteAddSneakers);
            EditSneakersCommand = new RelayCommand(ExecuteEditSneakers, CanExecuteEditSneakers);
            DeleteSneakersCommand = new RelayCommand(ExecuteDeleteSneakers, CanExecuteDeleteSneakers);
            ToggleFiltersCommand = new RelayCommand(ExecuteToggleFilters);

            LoadSneakers();
        }

        private void LoadSneakers()
        {
            try
            {
                _originalSneakers = new ObservableCollection<Sneaker>(_databaseService.GetSneakers());
                Sneakers = new ObservableCollection<Sneaker>(_originalSneakers);
                TotalCount = Sneakers.Count;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки кроссовок: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void SearchSneakers()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Sneakers = new ObservableCollection<Sneaker>(_originalSneakers);
            }
            else
            {
                var filteredList = _originalSneakers.Where(s =>
                    s.SneakersName.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.VendorCode.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Brand.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ProducingCountry.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.KindOfSport.NameSport.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Color.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Material.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Gender.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Size.ToString().IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                Sneakers = new ObservableCollection<Sneaker>(filteredList);
            }

            TotalCount = Sneakers.Count;
        }

        private void ExecuteAddSneakers(object parameter)
        {
            var window = new AddSneakersWindow()
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                LoadSneakers();
            }
        }

        private void ExecuteEditSneakers(object parameter)
        {
            if (parameter is Sneaker sneaker)
            {
                var sneakersCopy = new Sneaker
                {
                    CodSneakers = sneaker.CodSneakers,
                    SneakersName = sneaker.SneakersName,
                    VendorCode = sneaker.VendorCode,
                    Brand = sneaker.Brand,
                    ProducingCountry = sneaker.ProducingCountry,
                    KindOfSport = sneaker.KindOfSport != null ? new KindSport { CodKindSport = sneaker.KindOfSport.CodKindSport, NameSport = sneaker.KindOfSport.NameSport } : null,
                    Material = sneaker.Material,
                    Gender = sneaker.Gender,
                    Size = sneaker.Size,
                    Color = sneaker.Color,
                    UnitPrice = sneaker.UnitPrice,
                    Photo = sneaker.Photo
                };

                var editWindow = new EditSneakersWindow(sneakersCopy)
                {
                    Owner = Application.Current.MainWindow
                };

                if (editWindow.ShowDialog() == true)
                {
                    LoadSneakers();
                }
            }
        }

        private bool CanExecuteEditSneakers(object parameter)
        {
            return parameter is Sneaker;
        }

        private void ExecuteDeleteSneakers(object parameter)
        {
            if (SelectedSneaker != null && MessageBox.Show($"Вы уверены, что хотите удалить кроссовки '{SelectedSneaker.SneakersName}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteSneakers(SelectedSneaker.CodSneakers);
                    LoadSneakers();
                    
                    MessageBox.Show($"Кроссовки '{SelectedSneaker.SneakersName}' успешно удалены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении кроссовок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanExecuteDeleteSneakers(object parameter)
        {
            return parameter is Sneaker;
        }

        private void ExecuteToggleFilters(object parameter)
        {
            if (_filterWindow == null || !_filterWindow.IsLoaded)
            {
                _filterWindow = new FilterSneakersWindow(this)
                {
                    Owner = Application.Current.MainWindow
                };

                _filterWindow.Closed += (s, e) =>
                {
                    _filterWindow = null;
                    LoadSneakers();
                };

                _filterWindow.Show();
            }
            else
            {
                _filterWindow.Activate();
            }
        }

        public void ApplyFilters(FilterSneakersViewModel filter)
        {
            _currentFilter = filter;
            var filteredList = _originalSneakers.AsQueryable();

            if (_currentFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(_currentFilter.BrandFilter))
                {
                    filteredList = filteredList.Where(a => a.Brand.IndexOf(_currentFilter.BrandFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (_currentFilter.SelectedKindSportFilter != null)
                {
                    filteredList = filteredList.Where(a => a.KindOfSport != null && a.KindOfSport.CodKindSport == _currentFilter.SelectedKindSportFilter.CodKindSport);
                }

                if (_currentFilter.SelectedGenderFilter != null)
                {
                    filteredList = filteredList.Where(a => a.Gender.IndexOf(_currentFilter.SelectedGenderFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (_currentFilter.SelectedSizeFilter != null)
                {
                    filteredList = filteredList.Where(a => a.Size.ToString().IndexOf(_currentFilter.SelectedSizeFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (!string.IsNullOrWhiteSpace(_currentFilter.ColorFilter))
                {
                    filteredList = filteredList.Where(a => a.Color.IndexOf(_currentFilter.ColorFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (_currentFilter.MinPriceFilter.HasValue)
                {
                    filteredList = filteredList.Where(a => a.UnitPrice >= _currentFilter.MinPriceFilter.Value);
                }

                if (_currentFilter.MaxPriceFilter.HasValue)
                {
                    filteredList = filteredList.Where(a => a.UnitPrice <= _currentFilter.MaxPriceFilter.Value);
                }
            }
            Sneakers = new ObservableCollection<Sneaker>(filteredList.ToList());
        }
    }
}