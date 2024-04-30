using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modum.Models.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.MainModel;
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
        private readonly IProductSizesService _productSizesService;
        private readonly IFirebaseService _firebaseService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region ConstructorHelper
        public AdminControllerHelper(IBannedUsersService banUserService,
            IWorkerService workerService,
            IProductSizesService productSizesService,
            IFirebaseService firebaseService,
            ICategoryService categoryService,
            ISubcategoryService subcategoryService,
            IMainCategoryService mainCategoryService,
            UserManager<ApplicationUser> userManager)
        {
            _banUserService = banUserService;
            _workerService = workerService;
            _userManager = userManager;
            _productSizesService = productSizesService;
            _firebaseService = firebaseService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _mainCategoryService = mainCategoryService;
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

        public async Task<string> AddUserToPosition(ApplicationUser user, string position)
        {
            if (user != null)
            {
                if (!await _workerService.DoesThisPersonAlreadyBelongToAPosition(user))
                {
                    var worker = new Worker();

                    worker.AppUser = user;
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
                    var worker = await _workerService.GetWorkerByUserIdAsync(user.Id);

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

        #region ApplicationStatisticsHelper

        public async Task<StatisticsViewModel> GetApplicationStatisticsViewModel(DateTime startDate, DateTime endDate)
        {
            var viewModel = new StatisticsViewModel();

            viewModel.AmountOfAdmins = _subcategoryService.IQueryableGetUsersThatAreAdmins().Count();
            viewModel.AmountOfWorkers = _subcategoryService.IQueryableGetUsersThatAreWorkers().Count();
            viewModel.AmountOfUsers = _userManager.Users.Count();

            viewModel.TotalIncomeBeforeTax = await GetStripeIncomeBetweenDates(startDate, endDate);
            viewModel.StripeTakeaway = await GetStripeTakeawayBetweenDates(startDate, endDate);

            var mostSoldProduct = await _productSizesService.GetMostBoughtProducts();
            var category = _categoryService.IQueryableGetAllAsync().Where(x => x.Id == mostSoldProduct.CategoryId).FirstOrDefault();
            var subcategory = _subcategoryService.IQueryableGetAllAsync().Where(x => x.Id == mostSoldProduct.SubcategoryId).FirstOrDefault();
            var mainCategory = _mainCategoryService.IQueryableGetAllAsync().Where(x => x.Id == mostSoldProduct.MainCategoryId).FirstOrDefault();

            viewModel.MostBoughtCategory = category != null ? category.Name : "";
            viewModel.MostBoughtSubcategory = subcategory != null ? subcategory.Name : "";
            viewModel.MostBoughtMaincategory =mainCategory != null ? mainCategory.Name : "";

            viewModel.AmountOfBlogsInApplication = await _firebaseService.GetAllBlogPostsCount();
            return viewModel;
        }

        private async Task<double> GetStripeIncomeBetweenDates(DateTime startDate, DateTime endDate)
        {
            var service = new ChargeService();
            var options = new ChargeListOptions
            {
                Created = new DateRangeOptions
                {
                    GreaterThanOrEqual = startDate,
                    LessThanOrEqual = endDate
                }
            };
            var charges = service.List(options);

            double totalIncome = charges.Data.Sum(charge => charge.Amount);

            double totalIncomeInDollars = totalIncome / 100.00;

            return totalIncomeInDollars;
        }

        private async Task<double> GetStripeTakeawayBetweenDates(DateTime startDate, DateTime endDate)
        {

            double totalFees = 0;

            var service = new ChargeService();
            var options = new ChargeListOptions
            {
                Created = new DateRangeOptions
                {
                    GreaterThanOrEqual = startDate,
                    LessThanOrEqual = endDate
                },
                Limit = 100
            };

            var charges = service.List(options);

            foreach (var charge in charges)
            {
                totalFees += charge.ApplicationFeeAmount ?? Math.Round((charge.Amount / 100.00) * 0.03, 2);
            }
            return totalFees;
        }
        #endregion
    }
}