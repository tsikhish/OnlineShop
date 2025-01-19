using Microsoft.AspNetCore.Mvc;
using OnlineShops.Models;
using OnlineShops.Models.DbModels;

namespace OnlineShops.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AdventureWorksLT2019Context _context;

        public ReportsController(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IActionResult Index(string reportName = "SalesByYearAndMonth")
        {
            var viewModel = new ReportsViewModel
            {
                SelectedReport = reportName,
                ReportData = GetReportData(reportName)
            };

            return View(viewModel);
        }

        private IEnumerable<dynamic> GetReportData(string reportName)
        {
            switch (reportName)
            {
                case "SalesByYearAndMonth":
                    return _context.SalesOrderHeaders
                        .GroupBy(s => new { s.OrderDate.Year, s.OrderDate.Month })
                        .Select(g => new
                        {
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            TotalSales = g.Sum(x => x.SubTotal)
                        }).ToList();

                case "SalesByProduct":
                    return _context.SalesOrderDetails
                        .GroupBy(s => s.ProductId)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            TotalSales = g.Sum(x => x.LineTotal)
                        }).ToList();

                case "SalesByProductCategory":
                    return _context.SalesOrderDetails
                        .Join(_context.Products, sod => sod.ProductId, p => p.ProductId, (sod, p) => new { sod, p })
                        .GroupBy(x => x.p.ProductCategoryId)
                        .Select(g => new
                        {
                            CategoryId = g.Key,
                            TotalSales = g.Sum(x => x.sod.LineTotal)
                        }).ToList();

                case "SalesByCustomerAndYear":
                    return _context.SalesOrderHeaders
                        .GroupBy(s => new { s.CustomerId, s.OrderDate.Year })
                        .Select(g => new
                        {
                            CustomerId = g.Key.CustomerId,
                            Year = g.Key.Year,
                            TotalSales = g.Sum(x => x.SubTotal)
                        }).ToList();

                case "SalesByCity":
                    return _context.SalesOrderHeaders
                        .Join(_context.Addresses, soh => soh.ShipToAddressId, a => a.AddressId, (soh, a) => new { soh, a })
                        .GroupBy(x => x.a.City)
                        .Select(g => new
                        {
                            City = g.Key,
                            TotalSales = g.Sum(x => x.soh.SubTotal)
                        }).ToList();

                case "Top10Customers":
                    return _context.SalesOrderHeaders
                        .GroupBy(s => s.CustomerId)
                        .Select(g => new
                        {
                            CustomerId = g.Key,
                            TotalSales = g.Sum(x => x.SubTotal)
                        })
                        .OrderByDescending(x => x.TotalSales)
                        .Take(10)
                        .ToList();

                case "Top10CustomersByYear":
                    return _context.SalesOrderHeaders
                        .GroupBy(s => new { s.CustomerId, s.OrderDate.Year })
                        .Select(g => new
                        {
                            CustomerId = g.Key.CustomerId,
                            Year = g.Key.Year,
                            TotalSales = g.Sum(x => x.SubTotal)
                        })
                        .OrderByDescending(x => x.TotalSales)
                        .Take(10)
                        .ToList();

                case "Top10Products":
                    return _context.SalesOrderDetails
                        .GroupBy(s => s.ProductId)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            TotalSales = g.Sum(x => x.LineTotal)
                        })
                        .OrderByDescending(x => x.TotalSales)
                        .Take(10)
                        .ToList();

                case "Top10ProductsByProfit":
                    return _context.SalesOrderDetails
                        .GroupBy(s => s.ProductId)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            TotalProfit = g.Sum(x => x.LineTotal) - g.Sum(x => x.UnitPrice * x.OrderQty)
                        })
                        .OrderByDescending(x => x.TotalProfit)
                        .Take(10)
                        .ToList();

                case "Top10ProductsByYear":
                    return _context.SalesOrderDetails
                        .Join(_context.SalesOrderHeaders, sod => sod.SalesOrderId, soh => soh.SalesOrderId, (sod, soh) => new { sod, soh })
                        .GroupBy(x => new { x.sod.ProductId, x.soh.OrderDate.Year })
                        .Select(g => new
                        {
                            ProductId = g.Key.ProductId,
                            Year = g.Key.Year,
                            TotalSales = g.Sum(x => x.sod.LineTotal)
                        })
                        .OrderByDescending(x => x.TotalSales)
                        .Take(10)
                        .ToList();

                default:
                    return new List<dynamic>();
            }
        }
    }
}
