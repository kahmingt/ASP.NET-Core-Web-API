using Microsoft.AspNetCore.Mvc;
using WebApi.Models;


namespace WebApi.Services
{
    public interface IProductService
    {
        public Task<ProductDetails> CreateProduct(ProductDetails productDetails);
        public Task<bool> DeleteProductById(int id);
        public Task<ProductDetails> GetProductDetailsById(int id);
        public Task<IEnumerable<ProductListing>> GetProductList();
        public Task<ProductDetails> UpdateProductDetailsById(int id, ProductDetails product);
    }
}
