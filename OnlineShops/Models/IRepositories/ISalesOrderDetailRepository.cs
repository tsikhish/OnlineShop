namespace OnlineShops.Models.IRepositories
{
    public interface ISalesOrderDetailRepository
    {
        IEnumerable<SalesOrderDetail> GetOrderDetailsByOrderId(int orderId);
        void AddOrderDetail(SalesOrderDetail orderDetail);
        void UpdateOrderDetail(SalesOrderDetail orderDetail);
        void DeleteOrderDetail(SalesOrderDetail orderDetail);
    }
}
