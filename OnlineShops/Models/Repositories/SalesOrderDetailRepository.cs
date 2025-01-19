using OnlineShops.Models.IRepositories;

namespace OnlineShops.Models.Repositories
{
    public class SalesOrderDetailRepository :ISalesOrderDetailRepository
    {
        private readonly AdventureWorksLT2019Context _context;

        public SalesOrderDetailRepository(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IEnumerable<SalesOrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.SalesOrderDetails
                .Where(d => d.SalesOrderId == orderId)
                .ToList();
        }

        public void AddOrderDetail(SalesOrderDetail orderDetail)
        {
            _context.SalesOrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }


        public void UpdateOrderDetail(SalesOrderDetail orderDetail)
        {
            _context.SalesOrderDetails.Update(orderDetail);
            _context.SaveChanges();
        }


        public void DeleteOrderDetail(SalesOrderDetail orderDetail)
        {
            _context.SalesOrderDetails.Remove(orderDetail);
            _context.SaveChanges();
        }
    }
}

