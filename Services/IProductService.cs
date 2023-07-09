using tutorial_api_2.Models;

namespace tutorial_api_2.Services
{
    public interface IProductService
    {
        void CreateProduct(Product product);
        void UpdateProduct(int id, Product product);
        void DeleteProduct(int id);
        Product GetProduct(int id);
        List<Product> GetAllProduct();
    }
}
