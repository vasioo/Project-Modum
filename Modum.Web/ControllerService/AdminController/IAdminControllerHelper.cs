using Modum.DataAccess.MainModel;
using Modum.Models.ViewModels;

namespace Modum.Services.Services.ControllerService.AdminController
{
    public interface IAdminControllerHelper
    {

        Task BanUserHelper(ApplicationUser user, string reasonOfBanning);
        Task<OrderLogViewModel> AdditionalOrderInformationHelper(string orderId);
        Task<string> AddUserToPosition(ApplicationUser user, string position);
        Task RemoveUserFromPosition(ApplicationUser user);
        Task<Worker> GetWorkerByUserIdHelper(string userId);
        IQueryable<Worker> GetAllWorkers();
    }
}
