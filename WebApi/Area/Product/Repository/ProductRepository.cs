using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using WebApi.Area.Product.Utility;
using WebApi.Shared.Database;
using WebApi.Shared.Database.Entity;
using WebApi.Shared.Repository;
using WebApi.Shared.Utility;

namespace WebApi.Area.Product.Repository
{
    public class ProductRepository : GenericRepository<Products>, IProductRepository
    {
        protected ApplicationDbContext _db { get; set; }
        private OperationHelper<Products> _operationHelper;

        public ProductRepository(
            ApplicationDbContext db,
            OperationHelper<Products> operationHelper)
            : base(db)
        {
            _db = db;
            _operationHelper = operationHelper;
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

        public async Task<List<Products>> GetProductListAsync(ProductQueryableParameter parameter)
        {
            var model = (IQueryable<Products>)GetAll(x => !x.IsDeleted).Include(x => x.Category);
            return model!;
        }

        public async Task UpdateProductDetailsByIdAsync(Products products)
        {
            _db.Entry(products).Property(x => x.ProductId).IsModified = false;

            Update(products);
        }
    }
}
