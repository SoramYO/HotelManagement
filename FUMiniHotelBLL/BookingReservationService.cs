using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;

namespace FUMiniHotelBLL
{
    public class BookingReservationService
    {
        private BookingReservationRepository _roomRepository = new();


        public List<BookingReservation> GetAllBookings()
        {
            return _roomRepository.GetAllBookings();
        }

        public List<BookingReservation> GetAllBookingsByCustomerId(int customerId)
        {
            return _roomRepository.GetAllBookingsByCustomerId(customerId);
        }

        public BookingReservation? GetBookingById(int booking)
        {
            return _roomRepository.GetBookingById(booking);
        }

    }
}
