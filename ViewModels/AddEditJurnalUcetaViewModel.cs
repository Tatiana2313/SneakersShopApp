using SneakersShopApp.Models;
using SneakersShopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;

namespace SneakersShopApp.ViewModels
{
    public class AddEditJurnalUcetaViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private JurnalUceta _journals;
        private readonly bool _isAddMode;
        private ObservableCollection<Sneaker> _sneakers;
        private ObservableCollection<string> _months;

        public JurnalUceta Journals
        {
            get => _journals;
            set { _journals = value; OnPropertyChanged(); }
        }

        public bool IsAddMode
        {
            get => _isAddMode;
        }

        public string WindowTitle
        {
            get => $"{(IsAddMode ? "Добавление" : "Редактирование")} записи в журнал учета";
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

        public ObservableCollection<string> Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged(nameof(Months));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddEditJurnalUcetaViewModel(JurnalUceta journal, bool isAddMode)
        {
            _databaseService = new DatabaseService();
            _isAddMode = isAddMode;
            _journals = journal ?? new JurnalUceta
            {
                MonthName = DateTime.Today.ToString("MMMM", CultureInfo.InvariantCulture)
            };

            Months = new ObservableCollection<string>
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

            LoadComboBoxData();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadComboBoxData()
        {
            try
            {
                Sneakers = new ObservableCollection<Sneaker>(_databaseService.GetSneakers());
                if (_journals.Sneakers != null && _journals.Sneakers.CodSneakers != 0)
                {
                    var selectedSneakers = Sneakers.FirstOrDefault(a => a.CodSneakers == _journals.Sneakers.CodSneakers);
                    if (selectedSneakers != null) _journals.Sneakers = selectedSneakers;
                    else _journals.Sneakers = null;
                }
                else _journals.Sneakers = null;
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
                if (_isAddMode) _databaseService.AddJurnalUceta(_journals);
                else _databaseService.UpdateJurnalUceta(_journals);

                var window = parameter as Window;
                if (window != null)
                    window.DialogResult = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении записи в журнал учета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
