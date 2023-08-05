using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Database;

namespace WebApi.Repository
{
    public abstract class GenericRepositoryBase<T> : IGenericRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _db { get; set; }

        public GenericRepositoryBase(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Return entire object list as IQueryable<T>.
        /// </summary>
        public IQueryable<T> GetAll() => _db.Set<T>().AsNoTracking();

        /// <summary>
        /// Return entire object list as IQueryable<T> with predicate.
        /// </summary>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression) => _db.Set<T>().Where(expression).AsNoTracking();

        /// <summary>
        /// Return single object as IQueryable<T>.
        /// </summary>
        public IQueryable<T> GetSingle() => _db.Set<T>().AsNoTracking();

        /// <summary>
        /// Return single object as IQueryable<T> with predicate.
        /// </summary>
        public IQueryable<T> GetSingle(Expression<Func<T, bool>> expression) => _db.Set<T>().Where(expression).AsNoTracking();

        /// <summary>
        /// Add new entity using DbSet<T>.Add(T entity)
        /// </summary>
        public void Create(T entity) => _db.Set<T>().Add(entity);

        /// <summary>
        /// Update entity using DbSet<T>.Update(T entity)
        /// </summary>
        public void Update(T entity) => _db.Set<T>().Update(entity);

        /// <summary>
        /// Delete entity using DbSet<T>.Remove(T entity)
        /// </summary>
        public void Delete(T entity) => _db.Set<T>().Remove(entity);
    }
}
