using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface IBannedUsersService : IBaseService<ShortUserModel>
    {
        Task<ShortUserModel> GetUserByUserId(string userId);
    }
}
