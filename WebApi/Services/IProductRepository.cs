using WebApi.Models;
using WebApi.Database.Entities;

namespace WebApi.Repository
{
    public interface IProductRepository : IGenericRepositoryBase<Products>
    {
        /// <summary>
        /// Create new product details asynchronously.
        /// </summary>
        Task CreateProductAsync(Products product);
        /// <summary>
        /// Delete a specified Product, if exist.
        /// </summary>
        Task DeleteProductByIdAsync(Products product);
        /// <summary>
        /// Finds and returns the product details, if any, with the specified product id.
        /// </summary>
        Task<Products> GetProductDetailsByIdAsync(int id);
        /// <summary>
        /// Get entire product list.
        /// </summary>
        Task<IEnumerable<Products>> GetProductListAsync();
        /// <summary>
        /// Update product details by id.
        /// </summary>
        Task UpdateProductDetailsByIdAsync(Products product);
    }
}
