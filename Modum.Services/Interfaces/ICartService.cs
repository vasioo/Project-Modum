using Modum.Models.BaseModels.Models.Payment;

namespace Modum.Services.Interfaces
{
    public interface ICartService : IBaseService<Cart>
    {
        Task<Cart> GetCartContainerByUserId(string userId);
        Task<int> GetCartIdByUserId(string userId);
    }
}
