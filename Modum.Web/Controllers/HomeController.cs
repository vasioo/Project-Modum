using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.ViewModels;
using Modum.Services.Services;
using Modum.Services.Services.ControllerService.HomeController;
using Modum.Web.Areas.Identity.Pages;
using Stripe.Checkout;
using System.Diagnostics;

namespace Modum.Web.Controllers
{
    public class HomeController : Controller
    {
        #region ConstructorAndFields
        private readonly IHomeControllerHelper _helper;
        UserManager<ApplicationUser> _userManager;

        public HomeController(IHomeControllerHelper helper, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _helper = helper;
        }

        #endregion

        #region Shop

        [Authorize]
        public async Task<IActionResult> Shop(string productId)
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);

            if (productId == null)
            {
                productId = "";
            }

            var viewModel = await _helper.ShopHelper(productId, user!);
            return View("~/Views/UserViews/Shop.cshtml", viewModel);
        }

        
        #endregion

        #region Checkout
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            if (user?.Id != null || user?.Id != "")
            {
                var options = await _helper.CheckoutHelper(user!, HttpContext);
                if (options.Mode == "payment")
                {
                    var service = new SessionService();
                    Session session = service.Create(options);
                    return new StatusCodeResult(303);
                }
            }
            var viewModel = await _helper.CartHelper(user!);

            return View("~/Views/UserViews/Cart.cshtml", viewModel);
        }

        [Authorize]
        public async Task<IActionResult> SuccessfullPayment(bool paymentSuccess)
        {
            if (!paymentSuccess)
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByEmailAsync(username);

                if (await _helper.ProcessPayment(user!))
                {
                    return View("~/Views/UserViews/SuccessfullPayment.cshtml");
                }
            }
            return RedirectToAction("GenderCallTemplate", "Home");

        }
        #endregion

        #region UserProducts

        public async Task<IActionResult> _UserProductsPartial(int? page, ProductFilter filter, string sortBy, int mainCategoryId, string searchProducts)
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);

            var viewModel = await _helper._UserProductsPartialHelper(page, filter, sortBy, mainCategoryId, searchProducts, user!);

            return View("~/Views/UserViews/_UserProductsPartial.cshtml", viewModel);
        }
        #endregion

        #region Favourites
        [Authorize]
        public async Task<JsonResult> AddToFavourites(int productId)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByNameAsync(username);

                await _helper.AddToFavouritesHelper(productId, user!);
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The item was added successfully" });
        }
        [Authorize]
        public async Task<JsonResult> RemoveFromFavourites(int productId, bool addToCart, string size)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByNameAsync(username);
                await _helper.RemoveFromFavourites(productId, addToCart, size, user!);
                if (addToCart && size != null && size != "")
                {
                    Task.WaitAny();
                    await AddToCart(productId, size);
                }
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The item was added successfully" });
        }
        [Authorize]
        public async Task<IActionResult> Favourites()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);

            var viewModel = await _helper.FavouritesHelper(user!);

            return View("~/Views/UserViews/Favourites.cshtml", viewModel);

        }
        #endregion

        #region Cart
        [Authorize]
        public async Task<JsonResult> AddToCart(int productId, string size)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByNameAsync(username);
                await _helper.AddToCartHelper(productId, size, user!);
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The item was added successfully" });
        }
        [Authorize]
        public async Task<JsonResult> RemoveFromCart(int productId, bool addToFavourites, string size)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByNameAsync(username);

                await _helper.RemoveFromCartHelper(productId, size, user!);

                if (addToFavourites && productId > 0)
                {
                    await AddToFavourites(productId);
                }
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The item was added successfully" });
        }
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username) ?? new ApplicationUser();

            var viewModel = await _helper.CartHelper(user);

            return View("~/Views/UserViews/Cart.cshtml", viewModel);
        }
        #endregion

        #region GenderCallTemplate
        public async Task<IActionResult> GenderCallTemplate(string category)
        {
            var viewModel = await _helper.GenderCallTemplateHelper(category);

            return View("~/Views/UserViews/GenderCallTemplate.cshtml", viewModel);
        }

        #endregion

        #region HelperMethods
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("~/Areas/Identity/Pages/Error.cshtml", new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotImplemented()
        {
            return View("~/Views/Home/NotImplemented.cshtml");
        }
        #endregion

    }
}