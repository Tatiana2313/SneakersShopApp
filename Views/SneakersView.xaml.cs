using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SneakersShopApp.ViewModels;
using SneakersShopApp.Models;

namespace SneakersShopApp.Views
{
    public partial class SneakersView : UserControl
    {
        public SneakersView()
        {
            InitializeComponent();
        }

        private void SneakerCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Sneaker sneaker)
            {
                var viewModel = (SneakersViewModel)DataContext;
                viewModel.SelectedSneaker = sneaker;
                ModalBackground.Visibility = Visibility.Visible;
            }
        }

        private void CloseModalButton_Click(object sender, RoutedEventArgs e)
        {
            ModalBackground.Visibility = Visibility.Collapsed;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ModalBackground.Visibility = Visibility.Collapsed;
        }
    }
}
