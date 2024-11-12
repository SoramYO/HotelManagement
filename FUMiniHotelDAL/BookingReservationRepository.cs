using FUMiniHotelDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelDAL
{
    public class BookingReservationRepository
    {
        private FuminiHotelManagementContext _context;

        public List<BookingReservation> GetAllBookings()
        {
            _context = new();
            return _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .ThenInclude(r => r.RoomType)
                .ToList();
        }

        public List<BookingReservation> GetAllBookingsByCustomerId(int customerId)
        {
            _context = new();
            return _context.BookingReservations
               .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .ThenInclude(r => r.RoomType)
                .Where(x => x.CustomerId == customerId)
                .ToList();
        }

        public BookingReservation? GetBookingById(int bookingId)
        {
            _context = new();
            var booking = _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .ThenInclude(r => r.RoomType)
                .FirstOrDefault(x => x.BookingReservationId == bookingId);
            return booking;
        }
    }
}
