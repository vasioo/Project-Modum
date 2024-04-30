using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.MainModel;
using Modum.Models.ViewModels;
using Stripe.Checkout;

namespace Modum.Services.Services.ControllerService.HomeController
{
    public interface IHomeControllerHelper
    {
        Task<ShopViewModel> ShopHelper(string productId, ApplicationUser user);
        Task<FavouritesViewModel> FavouritesHelper(ApplicationUser user);
        Task AddToFavouritesHelper(Guid productId, ApplicationUser user);
        Task RemoveFromFavourites(Guid productId, bool addToCart, string size, ApplicationUser user);
        Task<CartViewModel> CartHelper(ApplicationUser user);
        Task AddToCartHelper(Guid productId, string size, ApplicationUser user);
        Task RemoveFromCartHelper(Guid productId, string size, ApplicationUser user);
        Task<GenderCallTemplateViewModel> GenderCallTemplateHelper(string category,string userId);
        Task<_UserProductsPartialViewModel> _UserProductsPartialHelper(int? page, ProductFilter filter, string sortBy,string searchProducts, ApplicationUser user);
        Task<SessionCreateOptions> CheckoutHelper(ApplicationUser user, HttpContext context,string location,string deliveryPlan);
        Task<bool> ProcessPayment(ApplicationUser user,HttpContext context);
    }
}
