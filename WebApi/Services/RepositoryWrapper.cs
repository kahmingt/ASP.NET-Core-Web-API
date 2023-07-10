using WebApi.Data;

namespace WebApi.Services
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _db;
        private IProductService _productService;

        public RepositoryWrapper(ApplicationDbContext db)
        {
            _db = db;
        }

        public IProductService ProductService
        {
            get
            {
                _productService ??= new ProductService(_db);
                return _productService;
            }
        }

    }
}
