using WebApi.Area.Product.Utility;
using WebApi.Shared.Database.Entity;
using WebApi.Shared.Repository;
using WebApi.Shared.Utility;

namespace WebApi.Area.Product.Repository
{
    public interface IProductRepository : IGenericRepository<Products>
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
        Task<List<Products>> GetProductListAsync(ProductQueryableParameter parameter);

        /// <summary>
        /// Update product details by id.
        /// </summary>
        Task UpdateProductDetailsByIdAsync(Products product);

    }
}
