using Microsoft.AspNetCore.Mvc.Rendering;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.LTCs;

namespace Modum.Models.ViewModels
{
    public class AddProductViewModel
    { 
        public IEnumerable<MainCategory> MainCategoryList { get; set; } = Enumerable.Empty<MainCategory>();
        public IEnumerable<LTC> LTCs { get; set; } = Enumerable.Empty<LTC>();
        public SelectList CategoryList { get; set; }
        public SelectList SubcategoryList { get; set; }
        public Array Sizes { get; set; } = new string[0];
    }
}
