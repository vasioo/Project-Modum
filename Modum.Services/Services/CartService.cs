using Modum.DataAccess;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class CartService : BaseService<Cart>, ICartService
    {
        private DataContext _dataContext;

        public CartService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Task<Cart> GetCartContainerByUserId(string userId)
        {
            var userCartItems = _dataContext.Cart.FirstOrDefault(item => item.UserId == userId);
            return Task.FromResult(userCartItems!);
        }
        public Task<int> GetCartIdByUserId(string userId)
        {
            var cart = _dataContext.Cart.FirstOrDefault(item => item.UserId == userId);

            if (cart != null)
            {
                return Task.FromResult(cart.Id);
            }

            return Task.FromResult(0);
        }

    }
}
