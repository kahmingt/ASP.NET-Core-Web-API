using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Models;

namespace WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        private async Task<bool> CommitChangesAsync()
        {
            bool result = true;
            var strategy = _db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transaction = await _db.Database.BeginTransactionAsync();
                try
                {
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    result = false;
                    //throw new Exception(ex.Message);
                }
            });
            return result;
        }

        public async Task<ProductDetails> CreateProduct(ProductDetails productDetails)
        {
            var model = new Products
            {
                ProductId = productDetails.ProductID,
                ProductName = productDetails.ProductName,
                UnitPrice = productDetails.UnitPrice,
                UnitsInStock = (short)productDetails.UnitInStock,
                CategoryId = productDetails.CategoryID,
                SupplierId = productDetails.SupplierID,
                IsDeleted = false,
            };

            await _db.Products.AddAsync(model);
            bool result = await CommitChangesAsync();

            if (model == null || !result)
            {
                return null!;
            }
            else
            {
                return new ProductDetails()
                {
                    CategoryID = model.Category.CategoryId,
                    CategoryName = model.Category.CategoryName,
                    ProductID = model.ProductId,
                    ProductName = model.ProductName,
                    UnitPrice = model.UnitPrice.Value,
                    UnitInStock = model.UnitsInStock,
                };
            }
        }

        public async Task<bool> DeleteProductById(int id)
        {
            var model = await _db.Products
                .Where(m => m.ProductId == id && !m.IsDeleted)
                .FirstOrDefaultAsync();

            bool result = false;
            if (model != null)
            {
                model.IsDeleted = true;
                result = await CommitChangesAsync();
            }
            return result;
        }

        public async Task<ProductDetails> GetProductDetailsById(int id)
        {
            var model = await _db.Products
                .Where(m => m.ProductId == id && !m.IsDeleted)
                .Select(m => new ProductDetails()
                {
                    CategoryID = m.Category.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    ProductID = m.ProductId,
                    ProductName = m.ProductName,
                    SupplierID = m.Supplier.SupplierId,
                    SupplierName = m.Supplier.CompanyName,
                    UnitInStock = m.UnitsInStock,
                    UnitPrice = m.UnitsInStock
                })
                .FirstOrDefaultAsync();

            return model!;
        }

        public async Task<IEnumerable<ProductListing>> GetProductList()
        {
            return await _db.Products
                .Where(m => !m.IsDeleted)
                .Select(m => new ProductListing()
                {
                    CategoryID = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    ProductID = m.ProductId,
                    ProductName = m.ProductName,
                    UnitPrice = m.UnitPrice.Value,
                    UnitInStock = m.UnitsInStock
                })
                .ToListAsync();
        }

        public async Task<ProductDetails> UpdateProductDetailsById(int id, ProductDetails productDetails)
        {
            var model = await _db.Products
                .Where(m => m.ProductId == id && !m.IsDeleted)
                .FirstOrDefaultAsync();

            bool result = false;
            if (model != null)
            {
                model.CategoryId = productDetails.CategoryID;
                model.ProductName = productDetails.ProductName;
                model.SupplierId = productDetails.SupplierID;
                model.UnitPrice = productDetails.UnitPrice;
                model.UnitsInStock = productDetails.UnitInStock;
                result = await CommitChangesAsync();
            }

            if (model == null || !result)
            {
                return null!;
            }
            else
            {
                return new ProductDetails()
                {
                    CategoryID = model.Category.CategoryId,
                    CategoryName = model.Category.CategoryName,
                    ProductID = model.ProductId,
                    ProductName = model.ProductName,
                    UnitPrice = model.UnitPrice.Value,
                    UnitInStock = model.UnitsInStock,
                };
            }
        }
    }
}
