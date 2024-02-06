using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class BannedUsersService : BaseService<ShortUserModel>, IBannedUsersService
    {
        private readonly DataContext _dataContext;
        public BannedUsersService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Task<ShortUserModel> GetUserByUserId(string userId)
        {
            var user = _dataContext.BannedUsers.FirstOrDefault(usr => usr.UserId == userId);
            return Task.FromResult(user);
        }
    }
}
