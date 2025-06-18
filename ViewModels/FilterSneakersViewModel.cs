using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SneakersShopApp.Models;
using SneakersShopApp.Services;
using SneakersShopApp.ViewModels;

namespace SneakersShopApp.ViewModels
{
    public class FilterSneakersViewModel : BaseViewModel
    {
        private readonly SneakersViewModel _parentViewModel;
        private readonly DatabaseService _databaseService;
        private string _brandFilter;
        private KindSport _selectedKindSportFilter;
        private string _selectedGenderFilter;
        private string _selectedSizeFilter;
        private string _colorFilter;
        private decimal? _minPriceFilter;
        private decimal? _maxPriceFilter;
        private ObservableCollection<KindSport> _kindSport;
        private readonly System.Timers.Timer _debounceTimer = new System.Timers.Timer(300) { AutoReset = false };

        public string BrandFilter
        {
            get => _brandFilter;
            set
            {
                _brandFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public KindSport SelectedKindSportFilter
        {
            get => _selectedKindSportFilter;
            set
            {
                _selectedKindSportFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string SelectedGenderFilter
        {
            get => _selectedGenderFilter;
            set
            {
                _selectedGenderFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string SelectedSizeFilter
        {
            get => _selectedSizeFilter;
            set
            {
                _selectedSizeFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string ColorFilter
        {
            get => _colorFilter;
            set
            {
                _colorFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public decimal? MinPriceFilter
        {
            get => _minPriceFilter;
            set
            {
                _minPriceFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public decimal? MaxPriceFilter
        {
            get => _maxPriceFilter;
            set
            {
                _maxPriceFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public ObservableCollection<KindSport> KindSport
        {
            get => _kindSport;
            set
            {
                _kindSport = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public FilterSneakersViewModel(SneakersViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _databaseService = new DatabaseService();
            LoadFilterData();
            CloseCommand = new RelayCommand(Close);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
        }

        private void LoadFilterData()
        {
            try
            {
                KindSport = new ObservableCollection<KindSport>(_databaseService.GetKindOfSport());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных фильтров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            _debounceTimer.Stop();
            _debounceTimer.Elapsed -= (s, e) => _parentViewModel.ApplyFilters(this);
            _debounceTimer.Elapsed += (s, e) => _parentViewModel.ApplyFilters(this);
            _debounceTimer.Start();
        }

        private void Close(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ClearFilters(object parameter)
        {
            BrandFilter = null;
            SelectedKindSportFilter = null;
            SelectedGenderFilter = null;
            SelectedSizeFilter = null;
            ColorFilter = null;
            MinPriceFilter = null;
            MaxPriceFilter = null;
        }
    }
}