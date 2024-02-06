using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class FavouritesViewModel
    {
        public IEnumerable<Product> FavoriteProducts { get; set; } = new List<Product>();
        public Cart Cart { get; set; } = new Cart();
        public IEnumerable<Product> TenFavItems { get; set; } = new List<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brands> BasicBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Brands> PremiumBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Product> LastViewedProducts { get; set; } = Enumerable.Empty<Product>();
    }
}
