using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.IRepositories;

namespace OnlineShops.Models.Repositories
{
    public class SalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private AdventureWorksLT2019Context _context;

        public SalesOrderHeaderRepository(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IEnumerable<SalesOrderHeader> GetAllOrders()
        {
            return _context.SalesOrderHeaders
                .Include(o => o.SalesOrderDetails)
                .ToList();
        }


        public SalesOrderHeader GetOrderById(int id)
        {
            return _context.SalesOrderHeaders
                .Include(o => o.SalesOrderDetails)
                .FirstOrDefault(o => o.SalesOrderId == id);
        }


        public void AddOrder(SalesOrderHeader order)
        {
            if (order.OrderDate < new DateTime(1753, 1, 1))
            {
                order.OrderDate = DateTime.Now;
            }
            if (order.DueDate < new DateTime(1753, 1, 1))
            {
                order.DueDate = DateTime.Now.AddDays(7);
            }
            if (order.ShipDate.HasValue && order.ShipDate < new DateTime(1753, 1, 1))
            {
                order.ShipDate = null;
            }
            _context.SalesOrderHeaders.Add(order);
            _context.SaveChanges();
        }


        public void UpdateOrder(SalesOrderHeader order)
        {
            if (order.OrderDate < new DateTime(1753, 1, 1) || order.OrderDate > new DateTime(9999, 12, 31))
            {
                order.OrderDate = DateTime.Now;
            }

            if (order.DueDate < new DateTime(1753, 1, 1) || order.DueDate > new DateTime(9999, 12, 31))
            {
                order.DueDate = DateTime.Now;
            }


            if (order.ShipDate < new DateTime(1753, 1, 1) || order.ShipDate > new DateTime(9999, 12, 31))
            {
                order.ShipDate = DateTime.Now;
            }
            if (order.ModifiedDate < new DateTime(1753, 1, 1) || order.ModifiedDate > new DateTime(9999, 12, 31))
            {
                order.ModifiedDate = DateTime.Now;
            }

            _context.SalesOrderHeaders.Update(order);
            _context.SaveChanges();
        }




        public void DeleteOrder(SalesOrderHeader order)
        {

            foreach (var orderDetail in order.SalesOrderDetails)
            {
                _context.SalesOrderDetails.Remove(orderDetail);
            }

            _context.SalesOrderHeaders.Remove(order);
            _context.SaveChanges();
        }
    }
}
