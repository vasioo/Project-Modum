using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modum.Models.MainModel;
using Modum.Services.Services.ControllerService.AdminController;
using Modum.Web.Models.Models.Pagination;
using Stripe;

namespace Modum.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminControllerHelper _helper;
        #endregion

        #region Constructor
        public AdminController(UserManager<ApplicationUser> userManager, IAdminControllerHelper helper)
        {
            _userManager = userManager;
            _helper = helper;
        }
        #endregion

        #region ManageUsers

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUsers(int? page, string searchString, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var users = _userManager.Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(usr => usr.LastName.Contains(searchString) || usr.FirstName.Contains(searchString));
            }
            int pageSize = 30;
            var paginatedList = PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize);
            return View("~/Views/Admin/ManageUsers.cshtml", paginatedList);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> BanUser(string userId, string reasonOfBanning)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                await _helper.BanUserHelper(user!, reasonOfBanning);
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The user was banned successfully" });
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> MakeAdmin(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The user was given rights of admin" });
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> RemoveAdmin(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The user was given rights of admin" });
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<JsonResult> MakeWorker(string userId, string position)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var res = await _helper.AddUserToPosition(user!, position);
                    if (res == "")
                    {
                        await _userManager.AddToRoleAsync(user, "Worker");
                        return Json(new { status = true, Message = "The user now has worker's rights" });
                    }
                }
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            catch (Exception)
            {
                return Json(new { status = false, Message = "Error Conflicted" });
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<JsonResult> RemoveWorker(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    await _helper.RemoveUserFromPosition(user!);
                    await _userManager.RemoveFromRoleAsync(user, "Worker");
                    return Json(new { status = true, Message = "The user does not have worker's rights anymore" });
                }
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            catch (Exception)
            {
                return Json(new { status = false, Message = "Error Conflicted" });
            }
        }
        #endregion

        #region ManageStripePayments

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> StripePaymentOrders(int page, string searchString, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null || page < 1)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var service = new ChargeService();
            int pageSize = 20;

            var options = new ChargeListOptions
            {
                Expand = new List<string> { "data.source" },
                Limit = 100000000000
            };

            var charges = await service.ListAsync(options);

            var count = charges.Count();

            var optionsForSearching = new ChargeSearchOptions();

            if (!String.IsNullOrEmpty(searchString))
            {
                optionsForSearching.Query = $"metadata['key']: '{searchString}' OR billing_details[email]: '{searchString}' OR created: '{searchString}' OR amount_captured: {searchString}";
                service.Search(optionsForSearching);
            }

            var paginatedOrders = charges.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = PaginatedList<Charge>.CreateStripeCustomAsync(paginatedOrders, page, pageSize, count);

            return View("~/Views/Admin/StripePaymentOrders.cshtml", paginatedList);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> AdditionalOrderInformation(string orderId)
        {
            try
            {
                var orderHelper = await _helper.AdditionalOrderInformationHelper(orderId);

                if (orderHelper == null)
                {
                    return BadRequest(orderHelper);
                }

                return View("~/Views/Admin/AdditionalOrderInformation.cshtml", orderHelper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ManageWorkers
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageWorkers(int? page, string searchString, string currentFilter)
        {

            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var users = _helper.GetAllWorkers();


            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(usr => usr.AppUser.LastName.Contains(searchString) || usr.AppUser.FirstName.Contains(searchString));
            }

            int pageSize = 30;
            return View(PaginatedList<Worker>.CreateAsync(users, page ?? 1, pageSize));
        }

        public async Task<IActionResult> EditWorkerInformation(string userId)
        {
            var worker =await _helper.GetWorkerByUserIdHelper(userId);

            return View("~/Views/Admin/EditWorkerInformation.cshtml", worker);
        }

        public async Task<IActionResult> EditInformationForWorker(Worker worker)
        {
            var position = worker.Position;
            var userId = worker.AppUser.Id;
            await RemoveWorker(userId);
            await MakeWorker(userId, position);

            return View("~/Views/Admin/EditWorkerInformation.cshtml", worker);
        }
        #endregion

        #region ApplicationStatistics

        public async Task<IActionResult> ApplicationStatistics(DateTime startDate, DateTime endDate)
        {
            var viewModel = await _helper.GetApplicationStatisticsViewModel(DateTime.Now.AddMonths(-1),DateTime.Now);
            return View("~/Views/Admin/ApplicationStatistics.cshtml", viewModel);
        }

        #endregion
    }
}