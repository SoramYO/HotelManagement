using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;
using System.Windows.Controls;
namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for CustomerDetail.xaml
    /// </summary>
    public partial class CustomerDetail : Window
    {
        public Customer Customer { get; set; }
        private readonly CustomerService _customerService = new();
        public CustomerDetail()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (Customer != null)
            {
                Customer.CustomerFullName = FullNameTextBox.Text;
                Customer.EmailAddress = EmailTextBox.Text;
                Customer.Telephone = TelephoneTextBox.Text;
                Customer.CustomerBirthday = DateOnly.Parse(BirthdayPicker.Text);
                Customer.CustomerStatus = StatusCombox.SelectedIndex == 0 ? (byte?)1 : (byte?)0;
                Customer.Password = PasswordBox.Password;
                MessageBox.Show("Customer updated successfully");
                _customerService.UpdateCustomer(Customer);
                this.Close();
                return;
            }

            var customer = new Customer
            {
                CustomerFullName = FullNameTextBox.Text,
                EmailAddress = EmailTextBox.Text,
                Telephone = TelephoneTextBox.Text,
                CustomerBirthday = DateOnly.Parse(BirthdayPicker.Text),
                CustomerStatus = StatusCombox.SelectedIndex == 0 ? (byte?)1 : (byte?)0,
                Password = PasswordBox.Password,
            };
            MessageBox.Show("Customer added successfully");
            _customerService.AddCustomer(customer);
            this.Close();
        }
        public void ValidateInput()
        {
            if (string.IsNullOrEmpty(FullNameTextBox.Text))
            {
                MessageBox.Show("Please enter a full name");
                return;
            }
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Please enter an email address");
                return;
            }
            if (string.IsNullOrEmpty(TelephoneTextBox.Text))
            {
                MessageBox.Show("Please enter a telephone number");
                return;
            }
            if (string.IsNullOrEmpty(BirthdayPicker.Text))
            {
                MessageBox.Show("Please enter a birthday");
                return;
            }
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Please enter a password");
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Customer != null)
            {
                LableDetail.Text = "Edit Customer";
                FullNameTextBox.Text = Customer.CustomerFullName;
                EmailTextBox.Text = Customer.EmailAddress;
                TelephoneTextBox.Text = Customer.Telephone;
                BirthdayPicker.Text = Customer.CustomerBirthday.ToString();
                StatusCombox.SelectedIndex = Customer.CustomerStatus == 1 ? 0 : 1;
                PasswordBox.Password = Customer.Password;
            }
            else
            {
                LableDetail.Text = "Add Customer";
            }

        }
    }
}
