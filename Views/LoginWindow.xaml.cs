using SneakersShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SneakersShopApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();

            Loaded += (s, e) => LoginTextBox.Focus();
        }

        private void ForgetPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Если вы забыли пароль, обратитесь за помощью к администратору!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
