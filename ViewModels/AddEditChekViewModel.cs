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
    public class AddEditChekViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly bool _isAddMode;
        private Chek _chek;
        private ObservableCollection<Sneaker> _sneakers;

        public Chek Chek
        {
            get => _chek;
            set
            {
                _chek = value;
                OnPropertyChanged(nameof(Chek));
            }
        }

        public bool IsAddMode
        {
            get => _isAddMode;
        }

        public string WindowTitle
        {
            get => $"{(IsAddMode ? "Добавление" : "Редактирование")} чека";
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

        public AddEditChekViewModel(Chek chek, bool isAddMode)
        {
            _databaseService = new DatabaseService();
            _isAddMode = isAddMode;
            _chek = chek ?? new Chek
            {
                Sneakers = new Sneaker(),
                DateOfSale = DateTime.Now,
                SaleTime = DateTime.Now.TimeOfDay
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
                if (_chek.Sneakers != null && _chek.Sneakers.CodSneakers != 0)
                {
                    var selectedSneakers = Sneakers.FirstOrDefault(a => a.CodSneakers == _chek.Sneakers.CodSneakers);
                    if (selectedSneakers != null) _chek.Sneakers = selectedSneakers;
                    else _chek.Sneakers = null;
                }
                else _chek.Sneakers = null;
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
                if (_isAddMode)
                    _databaseService.AddChek(Chek);
                else
                    _databaseService.UpdateChek(Chek);

                var window = parameter as Window;
                if (window != null)
                    window.DialogResult = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении чека: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
