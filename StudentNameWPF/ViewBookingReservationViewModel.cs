using FUMiniHotelDAL.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StudentNameWPF
{
    public class ViewBookingReservationViewModel : INotifyPropertyChanged
    {
        private BookingReservation _booking;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewBookingReservationViewModel(BookingReservation booking)
        {
            _booking = booking;

        }

        public int BookingReservationId => _booking.BookingReservationId;
        public DateOnly BookingDate => _booking.BookingDate!.Value;
        public decimal TotalPrice => decimal.Parse(_booking.TotalPrice!.ToString()!);
        public string BookingStatus => _booking.BookingStatus == 1 ? "Active" : "Cancelled";
        public Customer Customer => _booking.Customer!;
        public ICollection<BookingDetail> BookingDetails => _booking.BookingDetails!;

        public ICommand CloseCommand { get; }


        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
