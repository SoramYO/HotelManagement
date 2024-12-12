using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for ViewBookingReservationWindow.xaml
    /// </summary>
    public partial class ViewBookingReservationWindow : Window
    {

        private BookingReservationService _service = new();
        public BookingReservation Booking { get; set; }

        public ViewBookingReservationWindow(BookingReservation booking)
        {
            InitializeComponent();
            Booking = booking;
            DataContext = new ViewBookingReservationViewModel(Booking);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {

            _service.UpdateBookingReservationStatus(Booking.BookingReservationId);
            MessageBox.Show("Check-in successful");
            Close();
        }

    }
}
