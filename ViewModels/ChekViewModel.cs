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
    public class ChekViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Chek> _cheks;
        private Chek _selectedChek;
        private readonly User _user;
        private string _searchTerm;

        public ObservableCollection<Chek> Cheks
        {
            get => _cheks;
            set { _cheks = value; OnPropertyChanged(); }
        }

        public Chek SelectedChek
        {
            get => _selectedChek;
            set { _selectedChek = value; OnPropertyChanged(); }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                SearchCheks();
            }
        }

        public bool IsAdmin => _user.NameRole == "admin";

        public ICommand AddChekCommand { get; private set; }
        public ICommand EditChekCommand { get; private set; }
        public ICommand DeleteChekCommand { get; private set; }

        public ChekViewModel(User user)
        {
            _user = user;
            _databaseService = new DatabaseService();

            AddChekCommand = new RelayCommand(AddChek);
            EditChekCommand = new RelayCommand(EditChek, CanEditOrDeleteChek);
            DeleteChekCommand = new RelayCommand(DeleteChek, CanEditOrDeleteChek);

            LoadCheks();
        }

        private void LoadCheks()
        {
            try
            {
                Cheks = new ObservableCollection<Chek>(_databaseService.GetCheks());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки чеков: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchCheks()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                LoadCheks();
            else
            {
                var searchedList = Cheks.Where(s =>
                    s.DateOfSale != null && s.DateOfSale.ToString("dd.MM.yyyy").ToLower().Contains(SearchTerm.ToLower()) ||
                    s.Sneakers != null && s.Sneakers.SneakersName.ToLower().Contains(SearchTerm.ToLower()) ||
                    s.NumberOfSneakers.ToString().IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                Cheks = new ObservableCollection<Chek>(searchedList);
            }
        }

        public void AddChek(object parameter)
        {
            var chek = new Chek
            {
                Sneakers = new Sneaker(),
                DateOfSale = DateTime.Now,
                SaleTime = DateTime.Now.TimeOfDay
            };
            var window = new AddEditChekWindow(chek, true)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                LoadCheks();
                MessageBox.Show($"Чек успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void EditChek(object parameter)
        {
            if (SelectedChek != null)
            {
                var chekCopy = new Chek
                {
                    CheckNumber = SelectedChek.CheckNumber,
                    DateOfSale = SelectedChek.DateOfSale,
                    SaleTime = SelectedChek.SaleTime,
                    Sneakers = SelectedChek.Sneakers != null ? new Sneaker
                    {
                        CodSneakers = SelectedChek.Sneakers.CodSneakers,
                        SneakersName = SelectedChek.Sneakers.SneakersName
                    } : null,
                    NumberOfSneakers = SelectedChek.NumberOfSneakers
                };

                var editWindow = new AddEditChekWindow(chekCopy, false)
                {
                    Owner = Application.Current.MainWindow
                };

                if (editWindow.ShowDialog() == true)
                {
                    LoadCheks();
                    MessageBox.Show($"Чек успешно отредактирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteChek(object parameter)
        {
            if (SelectedChek != null && MessageBox.Show($"Вы уверены, что хотите удалить чек №{SelectedChek.CheckNumber}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteChek(SelectedChek.CheckNumber);
                    MessageBox.Show($"Чек №{SelectedChek.CheckNumber} успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCheks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении чека: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDeleteChek(object parameter)
        {
            return SelectedChek != null;
        }
    }
}
