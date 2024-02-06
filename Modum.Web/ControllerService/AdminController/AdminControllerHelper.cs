using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Stripe;

namespace Modum.Services.Services.ControllerService.AdminController
{
    [Authorize(Roles = "Admin")]
    public class AdminControllerHelper : IAdminControllerHelper
    {
        #region FieldsHelper
        private readonly IBannedUsersService _banUserService;
        private readonly IWorkerService _workerService;
        #endregion

        #region ConstructorHelper
        public AdminControllerHelper(IBannedUsersService banUserService, IWorkerService workerService)
        {
            _banUserService = banUserService;
            _workerService = workerService;
        }
        #endregion

        #region ManageUsersHelper
        public async Task BanUserHelper(ApplicationUser user, string reasonOfBanning)
        {
            var shortUser = new ShortUserModel();
            shortUser.UserId = user.Id.ToString();
            shortUser.ReasonOfBanning = reasonOfBanning;

            var userToBan = await _banUserService.GetUserByUserId(user.Id);
            if (userToBan != null)
            {
                await _banUserService.RemoveAsync(userToBan.Id);
            }
            await _banUserService.AddAsync(shortUser);
        }

        public async Task<string> AddUserToPosition(ApplicationUser user,string position)
        {
            if (user!=null)
            {
                if (!await _workerService.DoesThisPersonAlreadyBelongToAPosition(user))
                {
                    var worker = new Worker();

                    worker.User = user;
                    worker.TimeSinceChangingThePosition = DateTime.Now;
                    worker.Salary = 0;
                    worker.Bonus = 0;
                    worker.Position = position;
                    worker.TimeSinceJoiningTheCompany = DateTime.Now;

                    await _workerService.AddAsync(worker);

                    return "";
                }
                return "There is a worker role assigned to that user!";
            }
            else
            {
                return "User cannot be null!";
            }
        }

        public async Task RemoveUserFromPosition(ApplicationUser user)
        {
            if (user != null)
            {
                if (await _workerService.DoesThisPersonAlreadyBelongToAPosition(user))
                {
                    var worker = _workerService.GetWorkerByUserIdAsync(user.Id);

                    if (worker != null)
                    {
                        await _workerService.RemoveAsync(worker.Id);
                    }
                }
            }
        }

        #endregion

        #region ManageOrdersHelper
        public async Task<OrderLogViewModel> AdditionalOrderInformationHelper(string orderId)
        {
            var orderLogVModel = new OrderLogViewModel();

            var service = new ChargeService();
            var order = service.Get(orderId);

            orderLogVModel.Charge = order;

            return orderLogVModel;

        }
        #endregion

        #region ManageWorkersHelper
        public async Task<Worker> GetWorkerByUserIdHelper(string userId)
        {
            return await _workerService.GetWorkerByUserIdAsync(userId);
        }
        public IQueryable<Worker> GetAllWorkers()
        {
            return _workerService.IQueryableGetAllAsync();
        }
        #endregion

    }
}