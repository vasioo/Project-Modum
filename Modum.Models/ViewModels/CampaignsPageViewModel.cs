using Modum.Models.BaseModels.Models.LTCs;

namespace Modum.Models.ViewModels
{
    public class CampaignsPageViewModel
    {
        public IEnumerable<LTC> LTCs{ get; set; } = Enumerable.Empty<LTC>();
        public int CartItemsForUser { get; set; }
    }
}
