﻿using SneakersShopApp.Models;
using SneakersShopApp.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SneakersShopApp.Views
{
    public partial class AddEditJurnalUcetaWindow : Window
    {
        private bool _hasValidationErrors;

        public AddEditJurnalUcetaWindow(JurnalUceta journal, bool isAddMode)
        {
            InitializeComponent();
            _hasValidationErrors = false;
            DataContext = new AddEditJurnalUcetaViewModel(journal, isAddMode);
            Loaded += AddEditJurnalUcetaWindow_Loaded;
        }

        private void AddEditJurnalUcetaWindow_Loaded(object sender, RoutedEventArgs e)
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

        private void NumberOfDeliveredSneakersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string numberText = textBox.Text;
            if (string.IsNullOrWhiteSpace(numberText))
            {
                ShowValidationError(textBox, "Количество поставленных кроссовок обязательно.");
            }
            else if (!int.TryParse(numberText, out int number) || number <= 0)
            {
                ShowValidationError(textBox, "Количество поставленных кроссовок должно быть положительным числом.");
            }
            else if (numberText.Length > 6)
            {
                ShowValidationError(textBox, "Количество поставленных кроссовок не должно превышать 6 цифр.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void NumberOfDeliveredSneakersTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string newText = textBox.Text + e.Text;
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9]") || newText.Length > 6;
        }

        private void NumberOfSneakersSoldTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string numberText = textBox.Text;
            if (string.IsNullOrWhiteSpace(numberText))
            {
                ShowValidationError(textBox, "Количество проданных кроссовок обязательно.");
            }
            else if (!int.TryParse(numberText, out int number) || number <= 0)
            {
                ShowValidationError(textBox, "Количество проданных кроссовок должно быть положительным числом.");
            }
            else if (numberText.Length > 6)
            {
                ShowValidationError(textBox, "Количество проданных кроссовок не должно превышать 6 цифр.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void NumberOfSneakersSoldTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
            var viewModel = DataContext as AddEditJurnalUcetaViewModel;
            if (viewModel == null) return;

            _hasValidationErrors = false;

            if (viewModel.Journals.Sneakers == null)
            {
                _hasValidationErrors = true;
            }

            if (string.IsNullOrWhiteSpace(viewModel.Journals.NumberOfDeliveredSneakers.ToString()) ||
                !int.TryParse(viewModel.Journals.NumberOfDeliveredSneakers.ToString(), out int delivered) ||
                delivered <= 0 ||
                viewModel.Journals.NumberOfDeliveredSneakers.ToString().Length > 6)
            {
                _hasValidationErrors = true;
            }

            if (string.IsNullOrWhiteSpace(viewModel.Journals.NumberOfSneakersSold.ToString()) ||
                !int.TryParse(viewModel.Journals.NumberOfSneakersSold.ToString(), out int sold) ||
                sold <= 0 ||
                viewModel.Journals.NumberOfSneakersSold.ToString().Length > 6)
            {
                _hasValidationErrors = true;
            }

            var saveButton = (Button)FindName("SaveButton");
            if (saveButton != null) saveButton.IsEnabled = !_hasValidationErrors;
        }
    }
}
