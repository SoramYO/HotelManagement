using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for BookingDetails.xaml
    /// </summary>
    public partial class BookingDetails : Window
    {
        public BookingReservation BookingReservation { get; set; }
        private BookingReservationService _bookingReservationService = new();
        private readonly BookingDetailsViewModel _viewModel;
        public BookingDetails(int bookingId)
        {
            InitializeComponent();
            _viewModel = new BookingDetailsViewModel(_bookingReservationService);
            DataContext = _viewModel;

            // Load booking details once the window is initialized
            _viewModel.LoadBookingDetails(bookingId);
        }

    }
}
