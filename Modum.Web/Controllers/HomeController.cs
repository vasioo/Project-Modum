using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modum.Models.MainModel;
using Modum.Models.BaseModels.Error;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Services.ControllerService.HomeController;
using Stripe.Checkout;
using System.Net;

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
        public async Task<IActionResult> Checkout(string location,string deliveryPlan)
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            if (user?.Id != null || user?.Id != "")
            {
                var options = await _helper.CheckoutHelper(user!, HttpContext, location,deliveryPlan);
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

                if (await _helper.ProcessPayment(user!,HttpContext))
                {
                    return View("~/Views/UserViews/SuccessfullPayment.cshtml");
                }
            }
            return RedirectToAction("GenderCallTemplate", "Home");

        }
        #endregion

        #region UserProducts

        public async Task<IActionResult> _UserProductsPartial(int? page, ProductFilter filter, string sortBy, string searchProducts)
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            ViewBag.SearchString = searchProducts;
            var viewModel = await _helper._UserProductsPartialHelper(page, filter, sortBy, searchProducts, user!);

            return View("~/Views/UserViews/_UserProductsPartial.cshtml", viewModel);
        }
        #endregion

        #region Favourites
        [Authorize]
        public async Task<JsonResult> AddToFavourites(Guid productId)
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
        public async Task<JsonResult> RemoveFromFavourites(Guid productId, bool addToCart, string size)
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
        public async Task<JsonResult> AddToCart(Guid productId, string size)
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
        public async Task<JsonResult> RemoveFromCart(Guid productId, bool addToFavourites, string size)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";
                var user = await _userManager.FindByNameAsync(username);

                await _helper.RemoveFromCartHelper(productId, size, user!);

                if (addToFavourites && productId != Guid.Empty)
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
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";
            var viewModel = await _helper.GenderCallTemplateHelper(category, userId);

            return View("~/Views/UserViews/GenderCallTemplate.cshtml", viewModel);
        }

        #endregion

        #region DeliveryLocation

        [Authorize]
        public async Task<IActionResult> DeliveryLocation()
        {
            var referringUrl = HttpContext.Request.Headers["Referer"].ToString();
            var currentDomain = HttpContext.Request.Host.Value.ToLower();
            var expectedURL = $"https://{currentDomain}/Home/Cart";
            var expectedDeliveryReload = $"https://{currentDomain}/Home/Delivery";

            if (!string.Equals(referringUrl, expectedURL, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(referringUrl, expectedDeliveryReload, StringComparison.OrdinalIgnoreCase))
            {
                return Error(401);
            }


            return View("~/Views/UserViews/DeliveryLocation.cshtml");
        }

        #endregion

        #region HelperMethods

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? errorCode = null)
        {
            var errorMessage = GetHttpStatusMessage(errorCode ?? HttpContext.Response.StatusCode);

            var customErrorModel = new CustomErrorModel
            {
                StatusCode = errorCode ?? HttpContext.Response.StatusCode,
                CustomErrorMessage = errorMessage
            };
            return View("~/Views/Home/Error.cshtml", customErrorModel);
        }

        private string GetHttpStatusMessage(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.BadRequest => "Bad Request",
                (int)HttpStatusCode.Unauthorized => "Unauthorized",
                (int)HttpStatusCode.Forbidden => "Forbidden",
                (int)HttpStatusCode.NotFound => "Not Found",
                (int)HttpStatusCode.InternalServerError => "Server Error",
                (int)HttpStatusCode.NotImplemented => "Server Error",
                (int)HttpStatusCode.BadGateway => "Bad Gateway",
                (int)HttpStatusCode.ServiceUnavailable => "Currently Unavailable",
                (int)HttpStatusCode.LoopDetected => "Detected Loop",
                _ => "Unknown Error"
            };
        }

        public IActionResult NotImplemented()
        {
            return View("~/Views/Home/NotImplemented.cshtml");
        }

        #endregion
    }
}