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

        public void UpdateBookingReservationStatus(int id)
        {
            var update = _roomRepository.GetById(id);
            if (update != null)
            {
                update.BookingStatus = 1;
                _roomRepository.Update(update);
            }
        }

    }
}
