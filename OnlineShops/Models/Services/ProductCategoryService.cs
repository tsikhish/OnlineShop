using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.DbModels.DTODbModels;
using OnlineShops.Models.IRepositories;
using OnlineShops.Models.IService;

namespace OnlineShops.Models.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryService(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProductCategoryDTO> GetAllProductCategories()
        {
            return _repository.SelectCategories();
        }

        public ProductCategory GetProductCategoryById(int id)
        {
            return _repository.GetById(id);
        }

        
        public ProductCategory UpdateForViewProductCategory(int id)
        {
           var updateRepos =  _repository.UpdateForView(id);
            return updateRepos;
        }
        public void UpdateProductCategory(int id,ProductCategory category)
        {
            _repository.Update(id, category);
        }
        public void DeleteProductCategory(int id)
        {
            _repository.Delete(id);
        }
        public IEnumerable<ProductCategory> SelectParents(int id)
        {
            return _repository.SelectParents(id);
        }
        public IEnumerable<object> SelectCategoryNames()
        {
            return _repository.SelectedNamesWithId();
        }
        public void CreateProductCategory(ProductCategory productCategory)
        {
            _repository.Add(productCategory);
        }
        public Task<ProductCategory> Details(int id)
        {
            return _repository.Details(id);
        }
        public IEnumerable<ProductCategory> ReturnParentCategories(int id)
        {
            return _repository.ReturnParentCategories(id);
        }
    }
}

