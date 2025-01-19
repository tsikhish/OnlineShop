using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.IRepositories;

namespace OnlineShops.Models.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AdventureWorksLT2019Context _context;

        public CustomerRepository(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public List<Customer> GetAllCustomersWithAddressCount()
        {
            return _context.Customers
                .Include(c => c.CustomerAddresses)
                .ThenInclude(ca => ca.Address)
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    EmailAddress = c.EmailAddress,
                    Phone = c.Phone,
                    CustomerAddresses = c.CustomerAddresses
                })
                .ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers
                .Include(c => c.CustomerAddresses)
                .ThenInclude(ca => ca.Address)
                .FirstOrDefault(c => c.CustomerId == id);
        }

        public void AddCustomer(Customer customer)
        {

            if (customer.CustomerAddresses != null && customer.CustomerAddresses.Any())
            {
                foreach (var customerAddress in customer.CustomerAddresses)
                {
                    // Ensure AddressType is set
                    customerAddress.AddressType = "DefaultType"; // Replace "DefaultType" with a valid type or logic

                    // Add the address to the context
                    _context.Add(customerAddress.Address);
                }
            }


            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer, List<int> addressIds)
        {
            var existingCustomer = _context.Customers
                .Include(c => c.CustomerAddresses)
                .FirstOrDefault(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer != null)
            {

                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.EmailAddress = customer.EmailAddress;
                existingCustomer.Phone = customer.Phone;


                existingCustomer.CustomerAddresses = addressIds
                    .Select(addressId => new CustomerAddress
                    {
                        CustomerId = customer.CustomerId,
                        AddressId = addressId
                    })
                    .ToList();

                _context.SaveChanges();
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}
