namespace OnlineShops.Models.IService
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomersWithAddressCount();
        Customer GetCustomerById(int id);
        Customer GetCustomerWithAddresses(int id);
        void AddCustomer(Customer customer, string rawPassword, string saltpassword);
        void UpdateCustomer(Customer customer, List<int> addressIds);
        void DeleteCustomer(int id);
    }
}
