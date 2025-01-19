using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;

namespace OnlineShops.Models.Services
{
    public class SalesOrderService :ISalesOrderService
    {
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public SalesOrderService(ISalesOrderHeaderRepository salesOrderHeaderRepository,
                                  ISalesOrderDetailRepository salesOrderDetailRepository,
                                  ICustomerRepository customerRepository,
                                  IProductRepository productRepository)
        {
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }


        public List<SalesOrderHeader> GetAllOrders()
        {
            return _salesOrderHeaderRepository.GetAllOrders().ToList();
        }


        public SalesOrderHeader GetOrderById(int id)
        {
            return _salesOrderHeaderRepository.GetOrderById(id);
        }


        public void CreateOrder(SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails)
        {

            orderHeader.SalesOrderDetails = orderDetails;

            _salesOrderHeaderRepository.AddOrder(orderHeader);

            foreach (var orderDetail in orderDetails)
            {
                _salesOrderDetailRepository.AddOrderDetail(orderDetail);
            }
        }


        public void UpdateOrder(SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails)
        {
            orderHeader.SalesOrderDetails = orderDetails;

            _salesOrderHeaderRepository.UpdateOrder(orderHeader);


            foreach (var orderDetail in orderDetails)
            {
                _salesOrderDetailRepository.UpdateOrderDetail(orderDetail);
            }
        }


        public void DeleteOrder(int id)
        {
            var order = _salesOrderHeaderRepository.GetOrderById(id);
            if (order != null)
            {

                var orderDetails = _salesOrderDetailRepository.GetOrderDetailsByOrderId(id);
                foreach (var orderDetail in orderDetails)
                {
                    _salesOrderDetailRepository.DeleteOrderDetail(orderDetail);
                }

                _salesOrderHeaderRepository.DeleteOrder(order);
            }
        }
    }
}

