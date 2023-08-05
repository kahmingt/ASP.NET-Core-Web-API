using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Database.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public class ProductRepository : GenericRepositoryBase<Products>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task CreateProductAsync(Products product)
        {
            Create(product);
        }


        public async Task DeleteProductByIdAsync(Products product)
        {
            product.IsDeleted = true;
            Update(product);
        }

        public async Task<Products> GetProductDetailsByIdAsync(int id)
        {
            var model = await GetSingle(x => x.ProductId == id && !x.IsDeleted)
                                .Include(x => x.Category)
                                .Include(x => x.Supplier)
                                .FirstOrDefaultAsync();
            return model!;
        }

        public async Task<IEnumerable<Products>> GetProductListAsync()
        {
            return await GetAll(x => !x.IsDeleted)
                                .Include(x=>x.Category)
                                .ToListAsync();
        }

        public async Task UpdateProductDetailsByIdAsync(Products products)
        {
            Update(products);
        }
    }
}
