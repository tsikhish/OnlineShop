using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;

namespace OnlineShops.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly AdventureWorksLT2019Context _context;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;

        public ProductService(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(p => p.ProductCategory).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Include(p => p.ProductCategory)
                                    .FirstOrDefault(p => p.ProductId == id);
        }

        public void Create(Product product)
        {

            if (product.SellStartDate == DateTime.MinValue)
            {
                product.SellStartDate = DateTime.Now;
            }


            if (product.ModifiedDate == DateTime.MinValue)
            {
                product.ModifiedDate = DateTime.Now;
            }


            if (string.IsNullOrWhiteSpace(product.ProductNumber))
            {
                throw new ArgumentException("ProductNumber cannot be null or empty.");

            }
            if (product.SellEndDate.HasValue && product.SellEndDate.Value < new DateTime(1753, 1, 1))
            {
                product.SellEndDate = null;
            }

            if (product.DiscontinuedDate.HasValue && product.DiscontinuedDate.Value < new DateTime(1753, 1, 1))
            {
                product.DiscontinuedDate = null;
            }
            _context.Products.Add(product);

            _context.SaveChanges();
        }



        public void Update(Product product)
        {
            if (product.SellStartDate == DateTime.MinValue)
            {
                product.SellStartDate = DateTime.Now;
            }


            if (product.ModifiedDate == DateTime.MinValue)
            {
                product.ModifiedDate = DateTime.Now;
            }


            if (string.IsNullOrWhiteSpace(product.ProductNumber))
            {
                throw new ArgumentException("ProductNumber cannot be null or empty.");


            }
            if (product.SellEndDate.HasValue && product.SellEndDate.Value < new DateTime(1753, 1, 1))
            {
                product.SellEndDate = null;
            }

            if (product.DiscontinuedDate.HasValue && product.DiscontinuedDate.Value < new DateTime(1753, 1, 1))
            {
                product.DiscontinuedDate = null;
            }


            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public bool DeleteConfirmed(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
            {
                return false;
            }


            _context.Products.Remove(product);


            _context.SaveChanges();

            return true;
        }
    }
}
