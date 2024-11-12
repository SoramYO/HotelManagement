using FUMiniHotelBLL;
using FUMiniHotelDAL.Models;
using System.Windows;
namespace StudentNameWPF
{
    /// <summary>
    /// Interaction logic for ManagerCustomersWindow.xaml
    /// </summary>
    public partial class ManagerCustomersWindow : Window
    {
        private readonly CustomerService _customerService = new();
        public ManagerCustomersWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            CustomerDataGrid.ItemsSource = null;
            CustomerDataGrid.ItemsSource = _customerService.GetAllCustomers();

        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerDetail customerDetail = new();
            customerDetail.ShowDialog();
            LoadData();
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = CustomerDataGrid.SelectedItem;
            if (selectedCustomer != null)
            {
                CustomerDetail customerDetail = new();
                customerDetail.Customer = selectedCustomer as Customer;
                customerDetail.ShowDialog();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.");
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = CustomerDataGrid.SelectedItem as Customer;
            if (customer != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this customer?", "Delete Customer", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    return;
                }
                _customerService.DeleteCustomer(customer);
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a customer to delete");
            }

        }
    }
}
