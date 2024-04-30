using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Services.Interfaces;

namespace Modum.Web.ControllerService.FooterController
{
    public class FooterControllerHelper : IFooterControllerHelper
    {
        private readonly ILTCService _ltcService;
        private readonly ICartService _cartService;

        public FooterControllerHelper(ILTCService  ltcService, ICartService cartService)
        {
            _ltcService = ltcService;
            _cartService = cartService;

        }

        public async Task<IEnumerable<LTC>> GetCampaignInformationData()
        {
            return await _ltcService.GetAllAsync();
        }

        public async Task<int> GetAmountOfCartItemsForUser(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var userCart = await _cartService.GetCartContainerByUserId(userId);
                if (userCart != null)
                {
                    return userCart.CartItems.Count();
                }
            }
            return 0;
        }
    }
}
