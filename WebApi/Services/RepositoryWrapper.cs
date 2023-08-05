using Microsoft.EntityFrameworkCore;
using WebApi.Database;

namespace WebApi.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _db;
        private IProductRepository _productRepository;

        public RepositoryWrapper(ApplicationDbContext db)
        {
            _db = db;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                _productRepository ??= new ProductRepository(_db);
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
