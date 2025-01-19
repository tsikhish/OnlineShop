using OnlineShops.Models.DbModels.DTODbModels;

namespace OnlineShops.Models.IService
{
    public interface IProductCategoryService
    {
        IEnumerable<ProductCategoryDTO> GetAllProductCategories(); //+
        ProductCategory GetProductCategoryById(int id);
        IEnumerable<object> SelectCategoryNames();
        void CreateProductCategory(ProductCategory productCategory);

        IEnumerable<ProductCategory> SelectParents(int id);
        Task<ProductCategory> Details(int id);


        ProductCategory UpdateForViewProductCategory(int id);
        IEnumerable<ProductCategory> ReturnParentCategories(int id);

        void UpdateProductCategory(int id, ProductCategory category);
        void DeleteProductCategory(int id);
    }
}
