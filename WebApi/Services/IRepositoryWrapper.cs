namespace WebApi.Repository
{
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// Northwind.Products database handler.
        /// </summary>
        IProductRepository ProductRepository { get; }

        Task CommitChangesAsync();

    }
}
