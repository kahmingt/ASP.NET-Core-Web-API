namespace WebApi.Repository
{
    public interface IGenericRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetSingle();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
