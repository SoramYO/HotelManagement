using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for RevenueReportWindow.xaml
    /// </summary>
    public partial class RevenueReportWindow : Window
    {

        public RevenueReportWindow(FuminiHotelManagementContext context)
        {
            InitializeComponent();
            DataContext = new RevenueReportViewModel(context);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
