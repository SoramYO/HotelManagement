using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;
using System.Windows.Controls;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for CustomerScreen.xaml
    /// </summary>
    public partial class CustomerScreen : Window
    {
        public Customer Customer { get; set; }
        private readonly BookingReservationService _bookingReservationService = new();
        private readonly CustomerService _customerService = new();

        public CustomerScreen()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerInfo customerInfo = new();
            customerInfo.Owner = this;
            customerInfo.Customer = Customer;
            Hide();
            customerInfo.ShowDialog();

            Close();
        }


        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            BookingNewReservation bookingNewReservation = new(Customer);
            bookingNewReservation.Customer = Customer;
            bookingNewReservation.ShowDialog();
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            BookingHistoryDataGrid.ItemsSource = null;
            BookingHistoryDataGrid.ItemsSource = _bookingReservationService.GetAllBookingsByCustomerId(Customer.CustomerId);
            Customer = _customerService.GetCustomerById(Customer.CustomerId);

        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new();
            changePassword.Customer = Customer;
            changePassword.ShowDialog();
            LoadData();

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("You have logged out.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);

            Customer = null;
            MainWindow main = new MainWindow();
            main.Show();

            Close();
        }

        private void TextBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

            var textBlock = sender as TextBlock;
            if (textBlock?.ContextMenu != null)
            {

                var isLoggedIn = true;
                foreach (MenuItem menuItem in textBlock.ContextMenu.Items)
                {
                    if (menuItem.Header.ToString() == "Change Password")
                    {
                        menuItem.IsEnabled = isLoggedIn;
                    }
                }
            }
        }

        private void BookingHistoryDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BookingHistoryDataGrid.SelectedItem is BookingReservation selectedBooking)
            {
                BookingDetails bookingDetails = new(selectedBooking.BookingReservationId);
                bookingDetails.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a booking to view details.", "No Booking Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
