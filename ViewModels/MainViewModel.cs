using System.Windows;
using System.Windows.Input;
using SneakersShopApp.Models;
using SneakersShopApp.Views;

namespace SneakersShopApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly User _user;
        private object _currentView;
        private string _currentViewName = "Shop";

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string CurrentViewName
        {
            get => _currentViewName;
            set
            {
                _currentViewName = value;
                OnPropertyChanged();
             
                OnPropertyChanged(nameof(IsSneakersActive));
                OnPropertyChanged(nameof(IsProvidersActive));
                OnPropertyChanged(nameof(IsTTNActive));
                OnPropertyChanged(nameof(IsOrdersActive));
                OnPropertyChanged(nameof(IsJurnalUcetaActive));
                OnPropertyChanged(nameof(IsShopActive));
                OnPropertyChanged(nameof(IsAdminPanelActive)); 
                OnPropertyChanged(nameof(IsReportActive)); 
            }
        }

        public bool IsSneakersActive => CurrentViewName == "Sneakers";
        public bool IsProvidersActive => CurrentViewName == "Providers";
        public bool IsTTNActive => CurrentViewName == "TTN";
        public bool IsOrdersActive => CurrentViewName == "Orders";
        public bool IsJurnalUcetaActive => CurrentViewName == "JurnalUceta";
        public bool IsShopActive => CurrentViewName == "Shop";
        public bool IsAdminPanelActive => CurrentViewName == "Admin Panel";
        public bool IsReportActive => CurrentViewName == "Report";

        public bool IsAdmin => _user.NameRole == "admin";
        public bool IsAdminOrSeller => _user.NameRole == "admin" || _user.NameRole == "seller";

        public ICommand NavigateSneakersCommand { get; }
        public ICommand NavigateProvidersCommand { get; }
        public ICommand NavigateAdminPanelCommand { get; }
        public ICommand NavigateTTNCommand { get; }
        public ICommand NavigateOrdersCommand { get; }
        public ICommand NavigateJurnalUcetaCommand { get; }
        public ICommand NavigateShopCommand { get; }
        public ICommand NavigateReportCommand { get; }
        public ICommand NavigateExitCommand { get; }

        public MainViewModel(User user)
        {
            _user = user;
            NavigateSneakersCommand = new RelayCommand(o => NavigateToSneakers());
            NavigateProvidersCommand = new RelayCommand(o => NavigateToProviders());
            NavigateAdminPanelCommand = new RelayCommand(o => NavigateToAdminPanel());
            NavigateTTNCommand = new RelayCommand(o => NavigateToTTNs());
            NavigateOrdersCommand = new RelayCommand(o => NavigateToOrders());
            NavigateJurnalUcetaCommand = new RelayCommand(o => NavigateToJurnalUceta());
            NavigateShopCommand = new RelayCommand(o => NavigateToShop());
            NavigateReportCommand = new RelayCommand(o => NavigateToReports());
            NavigateExitCommand = new RelayCommand(o => NavigateToExit());

            NavigateToShop();
        }

        private void NavigateToSneakers()
        {
            var view = new SneakersView
            {
                DataContext = new SneakersViewModel(_user)
            };
            CurrentView = view;
            CurrentViewName = "Sneakers";
        }

        private void NavigateToProviders()
        {
            var view = new ProviderView
            {
                DataContext = new ProviderViewModel()
            };
            CurrentView = view;
            CurrentViewName = "Providers";
        }

        private void NavigateToTTNs()
        {
            var view = new TTNView
            {
                DataContext = new TTNViewModel()
            };
            CurrentView = view;
            CurrentViewName = "TTN";
        }

        private void NavigateToOrders()
        {
            var view = new ChekView
            {
                DataContext = new ChekViewModel(_user)
            };
            CurrentView = view;
            CurrentViewName = "Orders";
        }

        private void NavigateToJurnalUceta()
        {
            var view = new JurnalUcetaView
            {
                DataContext = new JurnalUcetaViewModel()
            };
            CurrentView = view;
            CurrentViewName = "JurnalUceta";
        }

        private void NavigateToShop()
        {
            var view = new WelcomeView
            {
                DataContext = new WelcomeView()
            };
            CurrentView = view;
            CurrentViewName = "Shop";
        }

        private void NavigateToAdminPanel()
        {
            var view = new AdminPanelView
            {
                DataContext = new AdminPanelViewModel()
            };
            CurrentView = view;
            CurrentViewName = "Admin Panel";
        }
        private void NavigateToReports()
        {
            var view = new ReportView();
            CurrentView = view;
            CurrentViewName = "Report";
        }

        private void NavigateToExit()
        {
            var window = new LoginWindow();
            window.Show();
            Application.Current.Windows[0].Close();
        }
    }
}
