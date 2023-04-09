using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Interface
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product);
        Task DeleteProduct(int productId);
        IEnumerable<Product> GetAll(ProdudctQueryParametersModel queryParameters);
        Task<Product?> GetProductById(int productId);
        Task UpdateProduct(int productId, Product product);
    }
}