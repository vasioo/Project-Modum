using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class FavouritesViewModel
    {
        public IEnumerable<ProductSizesHelpingTable> FavoriteProducts { get; set; } = new List<ProductSizesHelpingTable>();
        public Cart Cart { get; set; } = new Cart();
        public IEnumerable<Product> TenFavItems { get; set; } = new List<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public List<string> BasicBrands { get; set; } = new List<string>();
        public List<string> PremiumBrands { get; set; } = new List<string>();
        public IEnumerable<Product> LastViewedProducts { get; set; } = Enumerable.Empty<Product>();
        public int CartItemsForUser { get; set; }

    }
}
