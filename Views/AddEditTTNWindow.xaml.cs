using SneakersShopApp.Models;
using System;
using SneakersShopApp.ViewModels;
using System.Text.RegularExpressions;
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
    public partial class AddEditTTNWindow : Window
    {
        private bool _hasValidationErrors;

        public AddEditTTNWindow(TTN ttn, bool isAddMode)
        {
            InitializeComponent();
            _hasValidationErrors = false;
            DataContext = new AddEditTTNViewModel(ttn, isAddMode);
            Loaded += AddEditTTNWindow_Loaded;
        }

        private void AddEditTTNWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void DatePostPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker == null) return;

            if (!datePicker.SelectedDate.HasValue)
            {
                ShowValidationError(datePicker, "Дата поступления обязательна.");
            }
            else if (datePicker.SelectedDate.Value > DateTime.Now)
            {
                ShowValidationError(datePicker, "Дата поступления не может быть в будущем.");
            }
            else if (datePicker.SelectedDate.Value < new DateTime(1900, 1, 1))
            {
                ShowValidationError(datePicker, "Дата поступления не может быть раньше 1900 года.");
            }
            else
            {
                ClearValidationError(datePicker);
            }
            UpdateSaveButtonState();
        }

        private void ProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedItem == null)
            {
                ShowValidationError(comboBox, "Выберите поставщика.");
            }
            else
            {
                ClearValidationError(comboBox);
            }
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
            var viewModel = DataContext as AddEditTTNViewModel;
            if (viewModel == null) return;

            _hasValidationErrors = false;

            if (viewModel.TTN.Provider == null)
                _hasValidationErrors = true;

            if (viewModel.TTN.Sneakers == null)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.TTN.NumberOfSneakers.ToString()) || !int.TryParse(viewModel.TTN.NumberOfSneakers.ToString(), out int numberOfSneakers) || numberOfSneakers <= 0 || viewModel.TTN.NumberOfSneakers.ToString().Length > 6)
                _hasValidationErrors = true;

            if (viewModel.TTN.DatePost > DateTime.Now || viewModel.TTN.DatePost < new DateTime(1900, 1, 1))
                _hasValidationErrors = true;

            var saveButton = (Button)FindName("SaveButton");
            if (saveButton != null)
                saveButton.IsEnabled = !_hasValidationErrors;
        }
    }
}
