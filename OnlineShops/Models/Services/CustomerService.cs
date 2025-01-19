using Microsoft.AspNetCore.Identity;
using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;

namespace OnlineShops.Models.Services
{
    public class CustomerService :ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAllCustomersWithAddressCount()
        {
            return _customerRepository.GetAllCustomersWithAddressCount();
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public Customer GetCustomerWithAddresses(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public void AddCustomer(Customer customer, string rawPassword, string saltpassword)
        {

            var hasher = new PasswordHasher<Customer>();
            saltpassword = customer.PasswordHash;
            customer.PasswordHash = hasher.HashPassword(customer, rawPassword);

            _customerRepository.AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer, List<int> addressIds)
        {
            _customerRepository.UpdateCustomer(customer, addressIds);
        }

        public void DeleteCustomer(int id)
        {
            _customerRepository.DeleteCustomer(id);
        }
    }
}

