using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelBLL
{
    public class BookingService
    {
        private readonly Repository<BookingReservation> _bookingRepository = new();
        private readonly Repository<BookingDetail> _bookingDetailRepository = new();
        private readonly Repository<RoomInformation> _roomRepository = new();

        private int GenerateNewBookingId(FuminiHotelManagementContext context)
        {
            // Get the maximum BookingReservationID from the database
            var maxId = context.BookingReservations
                .OrderByDescending(b => b.BookingReservationId)
                .Select(b => b.BookingReservationId)
                .FirstOrDefault();

            // Return max + 1, or 1 if no bookings exist
            return maxId + 1;
        }

        public void CreateBooking(BookingReservation booking, BookingDetail bookingDetail, decimal totalPrice)
        {
            try
            {
                using (var context = new FuminiHotelManagementContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. Generate new booking ID
                            int newBookingId = GenerateNewBookingId(context);

                            // 2. Create new booking instance
                            var newBooking = new BookingReservation
                            {
                                BookingReservationId = newBookingId,
                                BookingStatus = 0,
                                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                                CustomerId = booking.CustomerId,
                                TotalPrice = totalPrice
                            };

                            // Add booking
                            context.BookingReservations.Add(newBooking);
                            context.SaveChanges();

                            // 3. Create booking detail
                            var newBookingDetail = new BookingDetail
                            {
                                BookingReservationId = newBookingId,
                                RoomId = bookingDetail.RoomId,
                                StartDate = bookingDetail.StartDate,
                                EndDate = bookingDetail.EndDate,
                                ActualPrice = totalPrice
                            };

                            context.BookingDetails.Add(newBookingDetail);

                            // 4. Update room status
                            var room = context.RoomInformations
                                .FirstOrDefault(r => r.RoomId == bookingDetail.RoomId);

                            if (room == null)
                            {
                                throw new Exception("Room not found");
                            }

                            room.RoomStatus = 2;
                            context.Entry(room).State = EntityState.Modified;

                            // 5. Save all changes
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                throw new($"DbUpdateException: {ex.InnerException?.Message ?? ex.Message}");
                // Handle or log the exception details for further analysis
            }
            catch (Exception ex)
            {
                throw new($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void CancelBooking(int bookingId)
        {
            try
            {
                using (var context = new FuminiHotelManagementContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Get the booking with tracking
                            var booking = context.BookingReservations
                                .FirstOrDefault(b => b.BookingReservationId == bookingId);

                            if (booking == null)
                            {
                                throw new Exception("Booking not found");
                            }

                            // Update booking status to cancelled
                            booking.BookingStatus = 0; // Cancelled
                            context.Entry(booking).State = EntityState.Modified;

                            // Get associated booking details
                            var bookingDetails = context.BookingDetails
                                .Where(bd => bd.BookingReservationId == bookingId)
                                .ToList();

                            // Update room status for each booking detail
                            foreach (var detail in bookingDetails)
                            {
                                var room = context.RoomInformations
                                    .FirstOrDefault(r => r.RoomId == detail.RoomId);

                                if (room != null)
                                {
                                    room.RoomStatus = 1; // Available
                                    context.Entry(room).State = EntityState.Modified;
                                }
                            }

                            // Save all changes
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error cancelling booking: " + ex.Message);
            }
        }
    }
}