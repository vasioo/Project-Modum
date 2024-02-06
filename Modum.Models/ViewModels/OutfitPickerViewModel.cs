using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Models.ViewModels
{
    public class OutfitPickerViewModel
    {
        public IEnumerable<Product> TenFavItems { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brands> BasicBrands { get; set; } = Enumerable.Empty<Brands>();
        public IEnumerable<Brands> PremiumBrands { get; set; } = Enumerable.Empty<Brands>();
    }
}
