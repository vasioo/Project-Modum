using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class OrderService : BaseService<OrderLog>, IOrderService
    {
        private DataContext _dataContext;

        public OrderService(DataContext context) : base(context)
        {
            _dataContext = context;
        }
    }
}
