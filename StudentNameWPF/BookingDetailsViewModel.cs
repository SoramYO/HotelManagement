using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;

namespace StudentNameWPF
{
    public class BookingDetailsViewModel : ViewModelBase
    {
        private readonly BookingReservationService _bookingReservationService;
        private BookingReservation _bookingReservation;

        public BookingReservation BookingReservation
        {
            get { return _bookingReservation; }
            set
            {
                if (_bookingReservation != value)
                {
                    _bookingReservation = value;
                    OnPropertyChanged(nameof(BookingReservation));
                    OnPropertyChanged(nameof(BookingDetails));
                    OnPropertyChanged(nameof(Customer));
                    OnPropertyChanged(nameof(TotalPrice));
                    OnPropertyChanged(nameof(BookingDate));
                    OnPropertyChanged(nameof(BookingStatus));
                    OnPropertyChanged(nameof(Rooms));
                    OnPropertyChanged(nameof(RoomTypes));
                }
            }
        }

        public IEnumerable<BookingDetail> BookingDetails => BookingReservation?.BookingDetails;

        public Customer Customer => BookingReservation?.Customer;

        public decimal? TotalPrice => BookingReservation?.TotalPrice;

        public DateOnly? BookingDate => BookingReservation?.BookingDate;

        public byte? BookingStatus => BookingReservation?.BookingStatus;

        public IEnumerable<RoomInformation> Rooms => BookingDetails?.Select(bd => bd.Room);

        public IEnumerable<RoomType> RoomTypes => Rooms?.Select(r => r.RoomType).Distinct();

        public BookingDetailsViewModel(BookingReservationService bookingReservationService)
        {
            _bookingReservationService = bookingReservationService;
        }

        public void LoadBookingDetails(int bookingId)
        {
            BookingReservation = _bookingReservationService.GetBookingById(bookingId);
        }
    }
}
