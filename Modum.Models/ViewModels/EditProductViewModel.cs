using Microsoft.AspNetCore.Mvc.Rendering;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.ViewModels
{
    public class EditProductViewModel
    {
        public IEnumerable<MainCategory> MainCategoryList { get; set; } = Enumerable.Empty<MainCategory>();
        public IEnumerable<LTC> LTCs { get; set; } = Enumerable.Empty<LTC>();
        public SelectList CategoryList { get; set; }
        public SelectList SubcategoryList { get; set; }
        public Array Sizes { get; set; } = new string[0];

        public Product Product{ get; set; }
        public string CloudinaryImageContainerId { get; set; } = "";
    }
}
