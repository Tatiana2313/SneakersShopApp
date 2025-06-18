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
    public class TTNViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<TTN> _ttns;
        private TTN _selectedTTN;
        private string _searchTerm;

        public ObservableCollection<TTN> TTNs
        {
            get => _ttns;
            set { _ttns = value; OnPropertyChanged(); }
        }

        public TTN SelectedTTN
        {
            get => _selectedTTN;
            set { _selectedTTN = value; OnPropertyChanged(); }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                SearchTTNs();
            }
        }

        public ICommand AddTTNCommand { get; private set; }
        public ICommand EditTTNCommand { get; private set; }
        public ICommand DeleteTTNCommand { get; private set; }

        public TTNViewModel()
        {
            _databaseService = new DatabaseService();

            AddTTNCommand = new RelayCommand(AddTTN);
            EditTTNCommand = new RelayCommand(EditTTN, CanEditOrDeleteTTN);
            DeleteTTNCommand = new RelayCommand(DeleteTTN, CanEditOrDeleteTTN);

            LoadTTNs();
        }

        private void LoadTTNs()
        {
            try
            {
                TTNs = new ObservableCollection<TTN>(_databaseService.GetTTNs());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки ТТН: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void SearchTTNs()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                LoadTTNs();
            else
            {
                var searchedList = TTNs.Where(s =>
                    s.DatePost != null && s.DatePost.ToString("dd.MM.yyyy").ToLower().Contains(SearchTerm.ToLower()) ||
                    s.Provider != null && s.Provider.ProviderName.ToLower().Contains(SearchTerm.ToLower()) ||
                    s.Sneakers != null && s.Sneakers.SneakersName.ToLower().Contains(SearchTerm.ToLower()) ||
                    s.NumberOfSneakers.ToString().IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                TTNs = new ObservableCollection<TTN>(searchedList);
            }
        }

        public void AddTTN(object parameter)
        {
            var ttn = new TTN
            {
                Provider = new Provider(),
                Sneakers = new Sneaker(),
                DatePost = DateTime.Now
            };
            var window = new AddEditTTNWindow(ttn, true)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                LoadTTNs();
                MessageBox.Show($"Накладная успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void EditTTN(object parameter)
        {
            if (SelectedTTN != null)
            {
                var ttnCopy = new TTN
                {
                    NumDoc = SelectedTTN.NumDoc,
                    DatePost = SelectedTTN.DatePost,
                    Provider = SelectedTTN.Provider != null ? new Provider
                    {
                        CodProdiver = SelectedTTN.Provider.CodProdiver,
                        ProviderName = SelectedTTN.Provider.ProviderName,
                        Addres = SelectedTTN.Provider.Addres,
                        PhoneFax = SelectedTTN.Provider.PhoneFax,
                        RascetScet = SelectedTTN.Provider.RascetScet
                    } : null,
                    Sneakers = SelectedTTN.Sneakers != null ? new Sneaker
                    {
                        CodSneakers = SelectedTTN.Sneakers.CodSneakers,
                        SneakersName = SelectedTTN.Sneakers.SneakersName
                    } : null,
                    NumberOfSneakers = SelectedTTN.NumberOfSneakers
                };

                var editWindow = new AddEditTTNWindow(ttnCopy, false)
                {
                    Owner = Application.Current.MainWindow
                };

                if (editWindow.ShowDialog() == true)
                {
                    LoadTTNs();
                    MessageBox.Show($"Накладная успешно отредактирована.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteTTN(object parameter)
        {
            if (SelectedTTN != null && MessageBox.Show($"Вы уверены, что хотите удалить накладную №{SelectedTTN.NumDoc}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteTTN(SelectedTTN.NumDoc);
                    MessageBox.Show($"Накладная №{SelectedTTN.NumDoc} успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTTNs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении накладной: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDeleteTTN(object parameter)
        {
            return SelectedTTN != null;
        }
    }
}
