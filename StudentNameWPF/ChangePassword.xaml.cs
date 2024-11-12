using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private readonly AuthenticationService _authenticationService = new();
        public Customer Customer { get; set; }
        public ChangePassword()
        {
            InitializeComponent();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Text;
            string newPassword = NewPasswordBox.Text;
            string confirmPassword = ConfirmPasswordBox.Text;

            // Validate fields
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentPassword != Customer.Password)
            {
                MessageBox.Show("Current Password do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirmation do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            _authenticationService.ChangePassword(Customer.CustomerId, newPassword);
            Customer.Password = newPassword;
            CustomerScreen customerScreen = new();
            customerScreen.Customer = Customer;
            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
