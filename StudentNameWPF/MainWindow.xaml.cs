using FUMiniHotelBLL;
using System.Windows;

namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AuthenticationService _authenticationService = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = EmailTextBox.Text;
            var password = PasswordBox.Password;

            var loginResult = _authenticationService.Login(username, password);

            if (loginResult.IsSuccessful)
            {
                if (loginResult.IsAdmin)
                {
                    MessageBox.Show("Welcome Admin!");
                    AdminWindow adminWindow = new();
                    adminWindow.Show();
                    Close();
                }
                else
                {
                    CustomerScreen customerScreen = new();
                    customerScreen.Customer = loginResult.Customer;
                    customerScreen.Show();
                    Close();
                    MessageBox.Show($"Welcome {loginResult.Customer.CustomerFullName}!");
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }

        }
    }
}