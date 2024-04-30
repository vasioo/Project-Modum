using Modum.DataAccess;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class OrderService:BaseService<Order>,IOrderService
    {
        private DataContext _context;

        public OrderService(DataContext context):base(context)
        {
        }
    }
}
