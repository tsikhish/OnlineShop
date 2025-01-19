using Microsoft.AspNetCore.Mvc;
using OnlineShops.Models.IService;
using OnlineShops.Models;
using OnlineShops.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace OnlineShops.Controllers
{
    public class ProductController : Controller
    {
        
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly AdventureWorksLT2019Context _context;
        public ProductController(AdventureWorksLT2019Context context,IProductService productService, IProductCategoryService productCategoryService)
        {
            _context = context;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _productCategoryService.GetAllProductCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        product.ThumbNailPhoto = memoryStream.ToArray(); // Store image as byte[]
                    }
                    product.ThumbnailPhotoFileName = Path.GetFileName(imageFile.FileName);
                }

                _productService.Create(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _productCategoryService.GetAllProductCategories();
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _productCategoryService.GetAllProductCategories();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        product.ThumbNailPhoto = memoryStream.ToArray();
                    }
                    product.ThumbnailPhotoFileName = Path.GetFileName(imageFile.FileName);
                }

                _productService.Update(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _productCategoryService.GetAllProductCategories();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = _productService.GetAll()
                    .FirstOrDefault(c => c.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
