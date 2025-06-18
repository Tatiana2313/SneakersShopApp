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
    public partial class FilterSneakersWindow : Window
    {
        public FilterSneakersWindow(SneakersViewModel viewModel)
        {
            InitializeComponent();

            DataContext = new FilterSneakersViewModel(viewModel);

            Loaded += (s, e) =>
            {
                if (Owner != null)
                {
                    Left = Owner.Left - Width - 5;
                    Top = Owner.Top + (Owner.Height - Height) / 2;
                    if (Left < 0) Left = 5;
                }
                else
                {
                    Left = 5;
                    Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
                }
            };
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[0-9.]") || (e.Text == "." && (sender as TextBox).Text.Contains("."));
        }
    }
}
