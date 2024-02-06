using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class ShopViewModel
    {
        public Product Product { get; set; } = new Product();
        public Cart Cart { get; set; }
        public Favourites Favourites { get; set; } = new Favourites();
        public IEnumerable<Product> TenFavItems { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brands> BasicBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Brands> PremiumBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Product> LastViewedProducts { get; set; } = Enumerable.Empty<Product>();
    }
}
