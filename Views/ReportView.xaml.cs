using Stimulsoft.Report;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SneakersShopApp.Views
{
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
        }

        private void StatisticsSalesButton_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("../../Reports/StatisticsSalesSneakers.mrt");
            report.ShowWithWpf();
        }

        private void RevenueBySportButton_Click(object sender, RoutedEventArgs e)
        {
            var report = new StiReport();
            report.Load("../../Reports/RevenueBySport.mrt");
            report.ShowWithWpf();
        }
    }
}
