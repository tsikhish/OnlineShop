using Microsoft.EntityFrameworkCore;
using OnlineShops.Models.DbModels.DTODbModels;

namespace OnlineShops.Models.IRepositories
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> Details(int id);
        IEnumerable<ProductCategoryDTO> SelectCategories();
        IEnumerable<ProductCategory> GetAll();
        ProductCategory GetById(int id);
        ProductCategory UpdateForView(int id);
        IEnumerable<object> SelectedNamesWithId();
        void Update(int id,ProductCategory category);
        void Add(ProductCategory productCategory);
        IEnumerable<ProductCategory> ReturnParentCategories(int id);
        void Delete(int id);
        IEnumerable<ProductCategory> SelectParents(int id);
    }
}
