using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShops.Models;
using OnlineShops.Models.IService;

namespace OnlineShops.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly AdventureWorksLT2019Context _context;

        public CustomerController(ICustomerService customerService, AdventureWorksLT2019Context context)
        {
            _customerService = customerService;
            _context = context;
        }

        public IActionResult Index()
        {
            var customers = _customerService.GetAllCustomersWithAddressCount();
            return View(customers);
        }

        public IActionResult Details(int id)
        {
            var customer = _context.Customers
                .Include(c => c.CustomerAddresses)
                .ThenInclude(ca => ca.Address)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer, List<Address> addresses)
        {
            if (ModelState.IsValid)
            {
                string plainPassword = customer.PasswordHash;
                string salt = GenerateSalt(10);
                string hashedPassword = HashPassword(plainPassword, salt);

                customer.PasswordSalt = salt;
                customer.PasswordHash = hashedPassword;
                customer.Rowguid = Guid.NewGuid();
                customer.ModifiedDate = DateTime.UtcNow;

                if (addresses != null && addresses.Any())
                {
                    foreach (var address in addresses)
                    {
                        address.Rowguid = Guid.NewGuid();
                        address.ModifiedDate = DateTime.UtcNow;

                        customer.CustomerAddresses.Add(new CustomerAddress
                        {
                            Address = address
                        });
                    }
                }

                _customerService.AddCustomer(customer, salt, hashedPassword);

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers
                .Include(c => c.CustomerAddresses)
                .ThenInclude(ca => ca.Address)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers
                    .Include(c => c.CustomerAddresses)
                    .ThenInclude(ca => ca.Address)
                    .FirstOrDefault(c => c.CustomerId == id);

                if (existingCustomer == null)
                {
                    return NotFound();
                }
                UpdateCustomer(existingCustomer,customer);
                if (customer.CustomerAddresses != null && customer.CustomerAddresses.Any())
                {
                    RemoveAddress(existingCustomer,customer);
                    foreach (var customerAddress in customer.CustomerAddresses)
                    {
                        FindExistingAddress(existingCustomer, customerAddress);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }



        public IActionResult Delete(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _customerService.DeleteCustomer(id);
            return RedirectToAction(nameof(Index));
        }
        private void UpdateCustomer(Customer existingCustomer,Customer customer)
        {
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.EmailAddress = customer.EmailAddress;
            existingCustomer.ModifiedDate = DateTime.UtcNow;

        }
        private void RemoveAddress(Customer existingCustomer,Customer customer)
        {
            foreach (var existingAddress in existingCustomer.CustomerAddresses.ToList())
            {
                if (!customer.CustomerAddresses.Any(ca => ca.AddressId == existingAddress.AddressId))
                {
                    _context.CustomerAddresses.Remove(existingAddress);
                }
            }
        }
        private void FindExistingAddress(Customer existingCustomer,CustomerAddress customerAddress)
        {
            var existingAddress = existingCustomer.CustomerAddresses
                           .FirstOrDefault(ca => ca.AddressId == customerAddress.AddressId);

            if (existingAddress != null)
            {
                if (existingAddress.Address == null)
                {
                    existingAddress.Address = new Address();
                }
                UpdateExistingAddress(existingAddress, customerAddress);
                }
            else
            {
                AddAllCustomers(customerAddress);
                existingCustomer.CustomerAddresses.Add(customerAddress);
            }
        }
        private void AddAllCustomers(CustomerAddress customerAddress)
        {
            if (customerAddress.Address == null)
            {
                customerAddress.Address = new Address
                {
                    AddressLine1 = "Default AddressLine1",
                    City = "Default City",
                    StateProvince = "Default State",
                    PostalCode = "00000",
                    CountryRegion = "Default Country",
                    Rowguid = Guid.NewGuid(),
                    ModifiedDate = DateTime.UtcNow
                };
            }
            else
            {
                AddCustomer(customerAddress);
            }
            customerAddress.AddressType ??= "Default AddressType";
        }
        private void AddCustomer(CustomerAddress customerAddress)
        {
            customerAddress.Address.Rowguid = Guid.NewGuid();
            customerAddress.Address.ModifiedDate = DateTime.UtcNow;
            customerAddress.Address.AddressLine1 ??= "Default AddressLine1";
            customerAddress.Address.City ??= "Default City";
            customerAddress.Address.StateProvince ??= "Default State";
            customerAddress.Address.PostalCode ??= "00000";
            customerAddress.Address.CountryRegion ??= "Default Country";
        }
        private void UpdateExistingAddress(CustomerAddress customerAddress,CustomerAddress existingAddress)
        {
            existingAddress.Address.AddressLine1 = customerAddress.Address?.AddressLine1 ?? "Default AddressLine1";
            existingAddress.Address.AddressLine2 = customerAddress.Address?.AddressLine2;
            existingAddress.Address.City = customerAddress.Address?.City ?? "Default City";
            existingAddress.Address.StateProvince = customerAddress.Address?.StateProvince ?? "Default State";
            existingAddress.Address.PostalCode = customerAddress.Address?.PostalCode ?? "00000";
            existingAddress.Address.CountryRegion = customerAddress.Address?.CountryRegion ?? "Default Country";
            existingAddress.Address.ModifiedDate = DateTime.UtcNow;
            existingAddress.AddressType = customerAddress.AddressType ?? "Default AddressType";
        }
        private string GenerateSalt(int maxLength = 50)
        {
            byte[] salt = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string saltString = Convert.ToBase64String(salt);
            return saltString.Length > maxLength ? saltString.Substring(0, maxLength) : saltString;
        }

        private string HashPassword(string password, string salt)
        {
            if (salt.Length > 50)
            {
                salt = salt.Substring(0, 50);
            }

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] saltedPassword = System.Text.Encoding.UTF8.GetBytes(password + salt);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPassword));
            }
        }
    }
}

