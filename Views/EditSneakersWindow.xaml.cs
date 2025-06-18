using SneakersShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SneakersShopApp.Models;


namespace SneakersShopApp.Views
{
    public partial class EditSneakersWindow : Window
    {
        private bool _hasValidationErrors;
        private TextBox _photoTextBox;

        public EditSneakersWindow(Sneaker sneakers)
        {
            InitializeComponent();
            _hasValidationErrors = false;
            DataContext = new EditSneakersViewModel(sneakers);
            Loaded += EditSneakersWindow_Loaded;
        }

        private void EditSneakersWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _photoTextBox = FindName("PhotoTextBox") as TextBox;
            if (_photoTextBox != null)
            {
                var binding = _photoTextBox.GetBindingExpression(TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                    ValidatePhoto();
                }
            }
            UpdateSaveButtonState();
        }

        private void SneakerNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string sneakerName = textBox.Text;
            if (string.IsNullOrWhiteSpace(sneakerName))
            {
                ShowValidationError(textBox, "Название кроссовок обязательно.");
            }
            else if (sneakerName.Length > 50)
            {
                ShowValidationError(textBox, "Название кроссовок не должно превышать 50 символов.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void VendorCodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string vendorCode = textBox.Text;
            if (string.IsNullOrWhiteSpace(vendorCode))
            {
                ShowValidationError(textBox, "Артикул обязателен.");
            }
            else if (vendorCode.Length > 8)
            {
                ShowValidationError(textBox, "Артикул не должен превышать 8 символов.");
            }
            else if (vendorCode.Length < 8)
            {
                ShowValidationError(textBox, "Артикул должен содержать 8 символов (M4587012).");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void BrandTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string brand = textBox.Text;
            if (string.IsNullOrWhiteSpace(brand))
            {
                ShowValidationError(textBox, "Бренд обязателен.");
            }
            else if (brand.Length > 20)
            {
                ShowValidationError(textBox, "Бренд не должен превышать 20 символов.");
            }
            else if (!Regex.IsMatch(brand, @"^[a-zA-Z\s-]+$"))
            {
                ShowValidationError(textBox, "Бренд должен содержать только буквы, пробелы или дефисы.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void ProducingCountryTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string producingCountry = textBox.Text;
            if (string.IsNullOrWhiteSpace(producingCountry))
            {
                ShowValidationError(textBox, "Страна производитель обязательна.");
            }
            else if (producingCountry.Length > 50)
            {
                ShowValidationError(textBox, "Страна производитель не должна превышать 50 символов.");
            }
            else if (!Regex.IsMatch(producingCountry, @"^[a-zA-Z\s-]+$"))
            {
                ShowValidationError(textBox, "Страна производитель должна содержать только буквы, пробелы или дефисы.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void SportComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedItem == null)
            {
                ShowValidationError(comboBox, "Выберите вид спорта.");
            }
            else
            {
                ClearValidationError(comboBox);
            }
            UpdateSaveButtonState();
        }

        private void MaterialTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string material = textBox.Text;
            if (string.IsNullOrWhiteSpace(material))
            {
                ShowValidationError(textBox, "Материал обязателен.");
            }
            else if (material.Length > 40)
            {
                ShowValidationError(textBox, "Материал не должен превышать 40 символов.");
            }
            else if (!Regex.IsMatch(material, @"^[a-zA-Z\s-]+$"))
            {
                ShowValidationError(textBox, "Материал должен содержать только буквы, пробелы или дефисы.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (comboBox.SelectedValue == null)
            {
                ShowValidationError(comboBox, "Выберите пол.");
            }
            else
            {
                ClearValidationError(comboBox);
            }
            UpdateSaveButtonState();
        }

        private void SizeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string sizeText = textBox.Text;
            if (string.IsNullOrWhiteSpace(sizeText))
            {
                ShowValidationError(textBox, "Размер обязателен.");
            }
            else if (!int.TryParse(sizeText, out int size))
            {
                ShowValidationError(textBox, "Размер должен быть числом.");
            }
            else if (size < 30)
            {
                ShowValidationError(textBox, "Размер должен быть больше 30.");
            }
            else if (size > 48)
            {
                ShowValidationError(textBox, "Размер не должен превышать 48.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void ColorTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string color = textBox.Text;
            if (string.IsNullOrWhiteSpace(color))
            {
                ShowValidationError(textBox, "Цвет обязателен.");
            }
            else if (color.Length > 20)
            {
                ShowValidationError(textBox, "Цвет не должен превышать 20 символов.");
            }
            else if (!Regex.IsMatch(color, @"^[a-zA-Z\s-]+$"))
            {
                ShowValidationError(textBox, "Цвет должен содержать только буквы, пробелы или дефисы.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string priceText = textBox.Text;
            if (string.IsNullOrWhiteSpace(priceText))
            {
                ShowValidationError(textBox, "Цена обязательна.");
            }
            else if (!decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
            {
                ShowValidationError(textBox, "Цена должна быть числом (например, 123 или 123.45).");
            }
            else if (price <= 0)
            {
                ShowValidationError(textBox, "Цена должна быть больше 0.");
            }
            else if (price > 100000)
            {
                ShowValidationError(textBox, "Цена не должна превышать 100,000.");
            }
            else
            {
                ClearValidationError(textBox);
            }
            UpdateSaveButtonState();
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9,.]");
        }

        private void PriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void BrowsePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            ValidatePhoto();
            UpdateSaveButtonState();
        }

        private void PhotoTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidatePhoto();
            UpdateSaveButtonState();
        }

        private void ValidatePhoto()
        {
            if (_photoTextBox == null) return;

            string photo = _photoTextBox.Text;
            if (string.IsNullOrWhiteSpace(photo))
            {
                ShowValidationError(_photoTextBox, "Выбор изображения обязателен.");
            }
            else if (photo.Length > 70)
            {
                ShowValidationError(_photoTextBox, "Имя файла изображения не должно превышать 70 символов.");
            }
            else
            {
                ClearValidationError(_photoTextBox);
            }
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
            control.BorderBrush = (Brush)brushConverter.ConvertFrom("#E0E0E0");
            control.ToolTip = null;
            _hasValidationErrors = false;
        }

        private void UpdateSaveButtonState()
        {
            var viewModel = DataContext as AddSneakersViewModel;
            if (viewModel == null) return;

            _hasValidationErrors = false;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.SneakersName) || viewModel.Sneaker.SneakersName.Length > 50)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.VendorCode) || viewModel.Sneaker.VendorCode.Length > 8)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.Brand) || viewModel.Sneaker.Brand.Length > 20 || !Regex.IsMatch(viewModel.Sneaker.Brand, @"^[a-zA-Z\s-]+$"))
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.ProducingCountry) || viewModel.Sneaker.ProducingCountry.Length > 50 || !Regex.IsMatch(viewModel.Sneaker.ProducingCountry, @"^[a-zA-Z\s-]+$"))
                _hasValidationErrors = true;

            if (viewModel.Sneaker.KindOfSport == null)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.Material) || viewModel.Sneaker.Material.Length > 40 || !Regex.IsMatch(viewModel.Sneaker.Material, @"^[a-zA-Z\s-]+$"))
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.Gender))
                _hasValidationErrors = true;

            if (viewModel.Sneaker.Size <= 0 || viewModel.Sneaker.Size > 100)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.Color) || viewModel.Sneaker.Color.Length > 20 || !Regex.IsMatch(viewModel.Sneaker.Color, @"^[a-zA-Z\s-]+$"))
                _hasValidationErrors = true;

            if (viewModel.Sneaker.UnitPrice <= 0 || viewModel.Sneaker.UnitPrice > 100000)
                _hasValidationErrors = true;

            if (string.IsNullOrWhiteSpace(viewModel.Sneaker.Photo) || viewModel.Sneaker.Photo.Length > 70)
                _hasValidationErrors = true;

            if (SaveButton != null)
                SaveButton.IsEnabled = !_hasValidationErrors;
        }
    }
}
