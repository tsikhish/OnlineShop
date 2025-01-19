using Microsoft.AspNetCore.Mvc;
using OnlineShops.Models.IService;
using OnlineShops.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShops.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private AdventureWorksLT2019Context _context;

        public SalesOrderController(ISalesOrderService salesOrderService, ICustomerService customerService, IProductService productService, AdventureWorksLT2019Context context)
        {
            _salesOrderService = salesOrderService;
            _customerService = customerService;
            _productService = productService;
            _context = context;
        }

        public IActionResult Index()
        {
            var salesOrders = _context.SalesOrderHeaders
                                      .Include(o => o.Customer)
                                      .ToList();
            return View(salesOrders);
            //var orders = _salesOrderService.GetAllOrders();
            //return View(orders);
        }


        public IActionResult Details(int id)
        {
            var order = _salesOrderService.GetOrderById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }


        public IActionResult Create()
        {
            ViewBag.Customers = _customerService.GetAllCustomersWithAddressCount();
            ViewBag.Products = _productService.GetAll();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails)
        {
            if (ModelState.IsValid)
            {
                _salesOrderService.CreateOrder(orderHeader, orderDetails);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = _customerService.GetAllCustomersWithAddressCount();
            ViewBag.Products = _productService.GetAll();
            return View(orderHeader);
        }

        public IActionResult Edit(int id)
        {
            var order = _salesOrderService.GetOrderById(id);
            if (order == null)
                return NotFound();

            ViewBag.Customers = _customerService.GetAllCustomersWithAddressCount();
            ViewBag.Products = _productService.GetAll();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SalesOrderHeader orderHeader, List<SalesOrderDetail> orderDetails)
        {
            if (id != orderHeader.SalesOrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _salesOrderService.UpdateOrder(orderHeader, orderDetails);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = _customerService.GetAllCustomersWithAddressCount();
            ViewBag.Products = _productService.GetAll();
            return View(orderHeader);
        }


        public IActionResult Delete(int id)
        {
            var order = _salesOrderService.GetOrderById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _salesOrderService.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

