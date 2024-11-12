using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for CustomerInfo.xaml
    /// </summary>
    public partial class CustomerInfo : Window
    {
        public Customer Customer { get; set; }
        private readonly CustomerService _customerService = new();
        public CustomerInfo()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Customer.CustomerFullName = FullNameTextBox.Text;
            Customer.Telephone = PhoneTextBox.Text;
            Customer.EmailAddress = EmailTextBox.Text;
            Customer.CustomerBirthday = DateOnly.Parse(BirthdayDatePicker.Text);
            _customerService.UpdateCustomer(Customer);
            CustomerScreen customerScreen = new();
            customerScreen.Customer = Customer;
            MessageBox.Show("Profile Updated");
            Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmailTextBox.IsEnabled = false;
        }
    }
}
