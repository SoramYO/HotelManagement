using FUMiniHotelDAL;
using FUMiniHotelDAL.Models;
using Microsoft.Extensions.Configuration;

namespace FUMiniHotelBLL
{
    public class AuthenticationService
    {
        private readonly Repository<Customer> _customerRepository = new();


        public LoginResult Login(string username, string password)
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true, true)
                   .Build();
            var adminEmail = config["AdminAccount:Email"];
            var adminPassword = config["AdminAccount:Password"];

            if (username == adminEmail && password == adminPassword)
            {
                return new LoginResult
                {
                    IsSuccessful = true,
                    IsAdmin = true,
                    Customer = null
                };
            }


            var customer = _customerRepository.GetAll().FirstOrDefault(c => c.EmailAddress == username && c.Password == password);

            if (customer != null)
            {
                return new LoginResult
                {
                    IsSuccessful = true,
                    IsAdmin = false,
                    Customer = customer
                };
            }

            return new LoginResult
            {
                IsSuccessful = false,
                IsAdmin = false,
                Customer = null
            };
        }
        public void ChangePassword(int customerId, string newPassword)
        {
            var customer = _customerRepository.GetById(customerId);
            customer.Password = newPassword;
            _customerRepository.Update(customer);
        }
    }

    public class LoginResult
    {
        public bool IsSuccessful { get; set; }
        public bool IsAdmin { get; set; }
        public Customer Customer { get; set; }
    }
}