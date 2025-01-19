namespace OnlineShops.Models.IRepositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomersWithAddressCount();
        Customer GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer, List<int> addressIds);
        void DeleteCustomer(int id);

    }
}
