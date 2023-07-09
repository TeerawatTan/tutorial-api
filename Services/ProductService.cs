using tutorial_api_2.Data;
using tutorial_api_2.Models;

namespace tutorial_api_2.Services
{
    public class ProductService : IProductService
    {
        private readonly SampleDbContext _context;

        public ProductService(SampleDbContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product product)
        {
            Product prod = new Product();
            prod.Name = product.Name;
            prod.Description = product.Description;
            prod.Price = product.Price;
            prod.Quantity = product.Quantity;

            _context.Products.Add(prod);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product is null)
                return;

            _context.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetAllProduct()
        {
            var products = _context.Products.ToList();

            return products;
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product is null)
                product = new Product();

            return product;
        }

        public void UpdateProduct(int id, Product product)
        {
            var prod = _context.Products.Find(id);

            if (prod is null)
                return;

            prod.Name = product.Name;
            prod.Description = product.Description;
            prod.Price = product.Price;
            prod.Quantity = product.Quantity;

            _context.SaveChanges();
        }
    }
}
