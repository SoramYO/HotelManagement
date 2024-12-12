using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private FuminiHotelManagementContext context = new();
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            ManagerCustomersWindow managerCustomersWindow = new ManagerCustomersWindow();
            managerCustomersWindow.Show();
        }

        private void ManageRooms_Click(object sender, RoutedEventArgs e)
        {
            ManagerRoomWindow managerRoomWindow = new ManagerRoomWindow();
            managerRoomWindow.Show();
        }

        private void ManageReservations_Click(object sender, RoutedEventArgs e)
        {
            ManagerBookingReservationWindow managerBookingReservationWindow = new ManagerBookingReservationWindow();
            managerBookingReservationWindow.Show();
        }

        private void ReportGeneration_Click(object sender, RoutedEventArgs e)
        {
            RevenueReportWindow revenueReportWindow = new RevenueReportWindow(context);
            revenueReportWindow.Show();
        }
    }
}
