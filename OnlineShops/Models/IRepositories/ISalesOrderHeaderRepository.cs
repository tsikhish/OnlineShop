namespace OnlineShops.Models.IRepositories
{
    public interface ISalesOrderHeaderRepository
    {
        IEnumerable<SalesOrderHeader> GetAllOrders();
        SalesOrderHeader GetOrderById(int id);
        void AddOrder(SalesOrderHeader order);
        void UpdateOrder(SalesOrderHeader order);
        void DeleteOrder(SalesOrderHeader order);
    }
}
