namespace OnlineShops.Models.IService
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
        bool DeleteConfirmed(int id);

    }
}
