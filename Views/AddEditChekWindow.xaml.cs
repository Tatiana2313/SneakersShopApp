using SneakersShopApp.Models;
using SneakersShopApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SneakersShopApp.Views
{
    public partial class AddEditChekWindow : Window
    {
        private bool _hasValidationErrors;

        public AddEditChekWindow(Chek chek, bool isAddMode)
        {
            InitializeComponent();
            _hasValidationErrors = false;
            DataContext = new AddEditChekViewModel(chek, isAddMode);
            Loaded += AddEditChekWindow_Loaded;
        }

        private void AddEditChekWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void SneakersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedItem == null)
            {
                ShowValidationError(comboBox, "Выберите кроссовки.");
            }
            else
            {
                ClearValidationError(comboBox);
            }
            UpdateSaveButtonState();
        }

        private void NumberOfSneakersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string NumberOfSneakersText = textBox.Text;
            if (string.IsNullOrWhiteSpace(NumberOfSneakersText))
            {
                ShowValidationError(textBox, "Количество кроссовок обязательно.");
            }
            else if (!int.TryParse(NumberOfSneakersText, out int numberOfAlbums) || numberOfAlbums <= 0)
            {
                ShowValidationError(textBox, "Количество кроссовок должно быть положительным числом.");
            }
            else if (NumberOfSneakersText.Length > 6)
            {
                ShowValidationError(textBox, "Количество кроссовок не должно превышать 6 цифр.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void NumberOfSneakersTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string newText = textBox.Text + e.Text;
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9]") || newText.Length > 6;
        }

        private void ShowValidationError(Control control, string message)
        {
            control.BorderBrush = Brushes.Red;
            control.ToolTip = message;
            _hasValidationErrors = true;
        }

        private void ClearValidationError(Control control)
        {
            var brushConverter = new BrushConverter();
            control.BorderBrush = (Brush)brushConverter.ConvertFrom("#E9E9FF");
            control.ToolTip = null;
        }

        private void UpdateSaveButtonState()
        {
            var viewModel = DataContext as AddEditChekViewModel;
            if (viewModel == null) return;

            _hasValidationErrors = false;

            if (viewModel.Chek.Sneakers == null)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Chek.NumberOfSneakers.ToString()) || !int.TryParse(viewModel.Chek.NumberOfSneakers.ToString(), out int numberOfSneakers) || numberOfSneakers <= 0 || viewModel.Chek.NumberOfSneakers.ToString().Length > 6)
                _hasValidationErrors = true;

            var saveButton = (Button)FindName("SaveButton");
            if (saveButton != null) saveButton.IsEnabled = !_hasValidationErrors;
        }
    }
}
