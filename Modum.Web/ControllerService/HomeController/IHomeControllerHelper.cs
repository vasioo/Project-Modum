using Microsoft.AspNetCore.Http;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.ViewModels;
using Stripe.Checkout;

namespace Modum.Services.Services.ControllerService.HomeController
{
    public interface IHomeControllerHelper
    {
        Task<ShopViewModel> ShopHelper(string productId, ApplicationUser user);
        Task<FavouritesViewModel> FavouritesHelper(ApplicationUser user);
        Task AddToFavouritesHelper(int productId, ApplicationUser user);
        Task RemoveFromFavourites(int productId, bool addToCart, string size, ApplicationUser user);
        Task<CartViewModel> CartHelper(ApplicationUser user);
        Task AddToCartHelper(int productId, string size, ApplicationUser user);
        Task RemoveFromCartHelper(int productId, string size, ApplicationUser user);
        Task<GenderCallTemplateViewModel> GenderCallTemplateHelper(string category);
        Task<_UserProductsPartialViewModel> _UserProductsPartialHelper(int? page, ProductFilter filter, string sortBy, int mainCategoryId, string searchProducts, ApplicationUser user);
        Task<SessionCreateOptions> CheckoutHelper(ApplicationUser user, HttpContext context);
        Task<bool> ProcessPayment(ApplicationUser user);
    }
}
