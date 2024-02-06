using Modum.Models.BaseModels.Models.LTCs;

namespace Modum.Web.Models.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Brand { get; set; } = "";
        public IEnumerable<string> Sizes { get; set; } = new List<string>();
        public string Colour { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal DiscountFromPrice { get; set; } = 0;
        public string Description { get; set; } = "";
        public string ReturnPolicy { get; set; } = "";
        public int MainCategoryId { get; set; } = 0;
        public int CategoryId { get; set; } = 0;
        public int SubcategoryId { get; set; } = 0;
        public string ImageContainerId { get; set; } = "";
        public string Season { get; set; } = "";
        public List<int> LTCs { get; set; } = new List<int>();
    }
}
