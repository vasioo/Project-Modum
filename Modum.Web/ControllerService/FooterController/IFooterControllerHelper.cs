using Modum.Models.BaseModels.Models.LTCs;

namespace Modum.Web.ControllerService.FooterController
{
    public interface IFooterControllerHelper
    {
        Task<IEnumerable<LTC>> GetCampaignInformationData();
        Task<int> GetAmountOfCartItemsForUser(string userId);
    }
}
