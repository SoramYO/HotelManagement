using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;
namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for ManagerBookingReservationWindow.xaml
    /// </summary>
    public partial class ManagerBookingReservationWindow : Window
    {
        private BookingReservationService _reservationService = new();
        public ManagerBookingReservationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            BookingHistoryDataGrid.ItemsSource = null;
            BookingHistoryDataGrid.ItemsSource = _reservationService.GetAllBookings();

        }

        private void ViewBookingReservation_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = BookingHistoryDataGrid.SelectedItem as BookingReservation;
            if (selectedBooking != null)
            {
                var viewWindow = new ViewBookingReservationWindow(selectedBooking);
                viewWindow.Booking = selectedBooking;
                viewWindow.ShowDialog();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a booking to edit");
            }

        }
    }
}
