using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;

namespace FUMiniHotelBLL
{
    public class CustomerService
    {
        private readonly Repository<Customer> _customerRepository = new();

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }
        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetById(id);
        }
        public void AddCustomer(Customer customer)
        {
            _customerRepository.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepository.Update(customer);
        }
        public void DeleteCustomer(Customer customer)
        {
            _customerRepository.Delete(customer);
        }
    }
}
