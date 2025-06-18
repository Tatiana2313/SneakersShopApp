using SneakersShopApp.Models;
using SneakersShopApp.Services;
using SneakersShopApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SneakersShopApp.ViewModels
{
    public class JurnalUcetaViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<JurnalUceta> _journals;
        private JurnalUceta _selectedJournal;
        private string _searchTerm;

        public ObservableCollection<JurnalUceta> Journals
        {
            get => _journals;
            set { _journals = value; OnPropertyChanged(); }
        }

        public JurnalUceta SelectedJournal
        {
            get => _selectedJournal;
            set { _selectedJournal = value; OnPropertyChanged(); }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                SearchJournals();
            }
        }

        public ICommand AddJurnalUcetaCommand { get; private set; }
        public ICommand EditJurnalUcetaCommand { get; private set; }
        public ICommand DeleteJurnalUcetaCommand { get; private set; }

        public JurnalUcetaViewModel()
        {
            _databaseService = new DatabaseService();

            AddJurnalUcetaCommand = new RelayCommand(AddJurnalUceta);
            EditJurnalUcetaCommand = new RelayCommand(EditJurnalUceta, CanEditOrDeleteJurnalUceta);
            DeleteJurnalUcetaCommand = new RelayCommand(DeleteJurnalUceta, CanEditOrDeleteJurnalUceta);

            LoadJurnalUceta();
        }

        private void LoadJurnalUceta()
        {
            try
            {
                Journals = new ObservableCollection<JurnalUceta>(_databaseService.GetJurnalUceta());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки журнала учета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchJournals()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                LoadJurnalUceta();
            else
            {
                var searchedList = Journals.Where(s =>
                    s.MonthName.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Sneakers != null && s.Sneakers.SneakersName.ToLower().Contains(SearchTerm.ToLower()) ||
                    s.NumberOfDeliveredSneakers.ToString().IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.NumberOfSneakersSold.ToString().IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                Journals = new ObservableCollection<JurnalUceta>(searchedList);
            }
        }

        public void AddJurnalUceta(object parameter)
        {
            var journal = new JurnalUceta
            {
                Sneakers = new Sneaker(),
                MonthName = DateTime.Today.ToString("MMMM", CultureInfo.InvariantCulture)
            };
            var window = new AddEditJurnalUcetaWindow(journal, true)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                LoadJurnalUceta();
                MessageBox.Show($"Запись в журнал учета успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void EditJurnalUceta(object parameter)
        {
            if (SelectedJournal != null)
            {
                var journalCopy = new JurnalUceta
                {
                    MonthName = SelectedJournal.MonthName,
                    Sneakers = SelectedJournal.Sneakers != null ? new Sneaker
                    {
                        CodSneakers = SelectedJournal.Sneakers.CodSneakers,
                        SneakersName = SelectedJournal.Sneakers.SneakersName
                    } : null,
                    NumberOfDeliveredSneakers = SelectedJournal.NumberOfDeliveredSneakers,
                    NumberOfSneakersSold = SelectedJournal.NumberOfSneakersSold
                };

                var editWindow = new AddEditJurnalUcetaWindow(journalCopy, false)
                {
                    Owner = Application.Current.MainWindow
                };

                if (editWindow.ShowDialog() == true)
                {
                    LoadJurnalUceta();
                    MessageBox.Show($"Запись в журнале учета успешно отредактирована.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeleteJurnalUceta(object parameter)
        {
            if (SelectedJournal != null && MessageBox.Show($"Вы уверены, что хотите удалить запись за месяц '{SelectedJournal.MonthName}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _databaseService.DeleteJurnalUceta(SelectedJournal.MonthName, SelectedJournal.Sneakers.SneakersName);
                    LoadJurnalUceta();
                    MessageBox.Show($"Запись в журнале учета успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении записи в журнале учета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDeleteJurnalUceta(object parameter)
        {
            return SelectedJournal != null;
        }
    }
}
