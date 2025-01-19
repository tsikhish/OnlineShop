using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShops.Models;
using OnlineShops.Models.DbModels.DTODbModels;
using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;

namespace OnlineShops.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly AdventureWorksLT2019Context _context;  
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService, AdventureWorksLT2019Context context)
        {
            _productCategoryService = productCategoryService;
            _context = context;
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _context.ProductCategories
                                             .Include(c => c.Products) 
                                             .FirstOrDefaultAsync(c => c.ProductCategoryId == id);
                if (category != null)
                {
                    return View("Details", category); 
                }
                return NotFound("Item not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }
        public IActionResult Index()
        {
            try
            {
                var categories = _productCategoryService.GetAllProductCategories();
                return View(categories);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = _productCategoryService.GetAllProductCategories()
                    .FirstOrDefault(c => c.ProductCategoryId == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IActionResult DeleteConfirmed()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    return BadRequest("Cannot delete the category because it is referenced by a product.");
                }
                var category = await _context.ProductCategories.FindAsync(id);
                if (category != null)
                {
                    _context.ProductCategories.Remove(category);
                    await _context.SaveChangesAsync();
                    return View(); 
                }
                return NotFound("Item not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var productCategory = _productCategoryService.UpdateForViewProductCategory(id);
                ViewBag.ParentCategories = _productCategoryService.ReturnParentCategories(id);

                return View(productCategory);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductCategoryId,Name,ParentProductCategoryId")] ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                _productCategoryService.UpdateProductCategory(id,category);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentCategories = _productCategoryService.SelectParents(id);

            return View(category);
        }
        public IActionResult Create()
        {
            ViewBag.ParentCategories = _productCategoryService.SelectCategoryNames();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCategory model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = _productCategoryService.SelectCategoryNames();
                return View(model);
            }
            _productCategoryService.CreateProductCategory(model);
            return RedirectToAction("Index");
        }
    }

}

