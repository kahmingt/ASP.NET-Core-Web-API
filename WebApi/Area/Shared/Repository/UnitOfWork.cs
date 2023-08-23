using Microsoft.EntityFrameworkCore;
using WebApi.Area.Product.Repository;
using WebApi.Shared.Database;
using WebApi.Shared.Database.Entity;
using WebApi.Area.Product.Utility;

namespace WebApi.Shared.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IProductRepository _productRepository;
        private OperationHelper<Products> _operationHelper;

        public UnitOfWork(
            ApplicationDbContext db,
            OperationHelper<Products> operationHelper)
        {
            _db = db;
            _operationHelper = operationHelper;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                _productRepository ??= new ProductRepository(_db, _operationHelper);
                return _productRepository;
            }
        }

        public async Task CommitChangesAsync()
        {
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
                    throw new Exception(ex.Message);
                }
            });
        }
    }

}