using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class GenderCallTemplateViewModel
    {
        public string Category { get; set; } = "";
        public IEnumerable<Product> MostBoughtItems { get; set; } = new List<Product>();
        public IEnumerable<Product> TenFavItems { get; set; } = new List<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brands> BasicBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Brands> PremiumBrands { get; set; } = Enumerable.Empty<Brands>();
        public LTC LimitedTimeCampaign { get; set; } = new LTC();
        public IEnumerable<BlogPost> BlogPostsForTemplate{ get; set; } = Enumerable.Empty<BlogPost>();
    }
}
