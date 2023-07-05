using WebApi.Data;

namespace WebApi.Services
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _db;
        private IOrderService _orderService;

        public RepositoryWrapper(ApplicationDbContext db)
        {
            _db = db;
        }

        public IOrderService OrderService
        {
            get
            {
                _orderService ??= new OrderService(_db);
                return _orderService;
            }
        }
    }
}
