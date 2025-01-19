using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.DbModels.DTODbModels;
using OnlineShops.Models.IRepositories;

namespace OnlineShops.Models.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AdventureWorksLT2019Context _context;

        public ProductCategoryRepository(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _context.ProductCategories.ToList();
        }

        public ProductCategory GetById(int id)
        {
            return _context.ProductCategories.Find(id);
        }

        public IEnumerable<ProductCategory> SelectParents(int id){
            var select =  _context.ProductCategories
                            .Where(c => c.ProductCategoryId != id) 
                            .ToList();
            return select;
        }
        public void Create(ProductCategory model)
        {
            var newCategory = new ProductCategory
            {
                Name = model.Name,
                ParentProductCategoryId = model.ParentProductCategoryId
            };

            _context.ProductCategories.Add(newCategory);
            _context.SaveChanges();
        }
        
        public ProductCategory UpdateForView(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);
            if (productCategory == null)
            {
                throw new Exception("Not Found");
            }
            return productCategory;
        }
        public async Task<ProductCategory> Details(int id)
        {
            var category = await _context.ProductCategories
                                             .Include(c => c.Products)
                                             .FirstOrDefaultAsync(c => c.ProductCategoryId == id);

            return category;
        }

        public void Update(int id,ProductCategory category)
        {
            var existingCategory = _context.ProductCategories.Find(id);
            if (existingCategory == null)
            {
                throw new Exception("Not Found");
            }
            existingCategory.Name = category.Name;
            existingCategory.ParentProductCategoryId = category.ParentProductCategoryId;

            _context.SaveChanges();
        }
        public async void Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                var category = await _context.ProductCategories.FindAsync(id);
                if (category != null)
                {
                    _context.ProductCategories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Not Found");
                }
            }
            else
            {
                throw new Exception("Cannot delete category due to references.");
            }

        }
        public int GetProductCountForCategory(int categoryId)
        {
            return _context.Products.Count(p => p.ProductCategoryId == categoryId);
        }

        public IEnumerable<ProductCategory> GetAllCategoriesWithParent()
        {
            return _context.ProductCategories.ToList();
        }

        public IEnumerable<object> SelectedNamesWithId()
        {
            var productWithNames =  _context.ProductCategories
                                .Select(c => new 
                                {
                                    c.ProductCategoryId,
                                    c.Name
                                })
                                .ToList();
            return productWithNames;
        }
        public void Add(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            _context.SaveChanges();
        }

        public IEnumerable<ProductCategoryDTO> SelectCategories()
        {
            var categories = _context.ProductCategories
                            .Select(c => new ProductCategoryDTO
                            {
                                ProductCategoryId = c.ProductCategoryId,
                                Name = c.Name,
                                Count = c.Products.Count
                            })
                            .ToList();
            if (categories == null) throw new Exception("Not Found");
            return categories;
        }
        public IEnumerable<ProductCategory> ReturnParentCategories(int id)
        {
            var parentCategories = _context.ProductCategories
                            .Where(c => c.ProductCategoryId != id)
                            .ToList();
            return parentCategories;
        }
    }
}
