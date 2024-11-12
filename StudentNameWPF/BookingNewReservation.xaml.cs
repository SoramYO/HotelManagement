using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for BookingNewReservation.xaml
    /// </summary>
    public partial class BookingNewReservation : Window
    {
        public Customer Customer { get; set; }
        public BookingNewReservation(Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            DataContext = new BookingNewReservationViewModel(Customer);
        }

    }
}
