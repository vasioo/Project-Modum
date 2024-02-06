using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<CartItem> ProductSizes { get; set; } = Enumerable.Empty<CartItem>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brands> BasicBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Brands> PremiumBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Product> TenFavItems { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> LastViewedProducts { get; set; } = Enumerable.Empty<Product>();
    }
}
