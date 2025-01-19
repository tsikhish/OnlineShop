namespace OnlineShops.Models.IService
{
    public interface ISalesOrderService
    {
        List<SalesOrderHeader> GetAllOrders();

        SalesOrderHeader GetOrderById(int id);

        void CreateOrder(SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails);


        void UpdateOrder(SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails);

        void DeleteOrder(int id);
    }
}
