using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class OutfitPickerViewModel
    {
        public IEnumerable<Product> TenFavItems { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public List<string> BasicBrands { get; set; } = new List<string>();
        public List<string> PremiumBrands { get; set; } = new List<string>();
        public int CartItemsForUser { get; set; }
    }
}
