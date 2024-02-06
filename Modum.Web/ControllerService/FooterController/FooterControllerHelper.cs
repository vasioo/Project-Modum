using Modum.Models.BaseModels.Models.LTCs;
using Modum.Services.Interfaces;

namespace Modum.Web.ControllerService.FooterController
{
    public class FooterControllerHelper : IFooterControllerHelper
    {
        private readonly ILTCService _ltcService;

        public FooterControllerHelper(ILTCService  ltcService)
        {
            _ltcService = ltcService;
        }

        public async Task<IEnumerable<LTC>> GetCampaignInformationData()
        {
            return await _ltcService.GetAllAsync();
        }
    }
}
