using WebApi.Area.Product.Repository;
using WebApi.Shared.Database.Entity;

namespace WebApi.Shared.Repository
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Northwind <see cref="Products"/> database handler.
        /// </summary>
        IProductRepository ProductRepository { get; }

        Task CommitChangesAsync();

    }
}