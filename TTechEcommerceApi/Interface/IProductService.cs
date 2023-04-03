using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Interface
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        IEnumerable<Product> GetAll(ProdudctQueryParametersModel queryParameters);
        Task<Product?> GetProductById(int productId);
        Task<Product?> UpdateProduct(int productId, Product product);
    }
}