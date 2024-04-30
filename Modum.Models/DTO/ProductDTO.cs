using Modum.Models.BaseModels.Models.LTCs;

namespace Modum.Web.Models.Models.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Brand { get; set; } = "";
        public IEnumerable<string> Sizes { get; set; } = new List<string>();
        public string Colour { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal DiscountFromPrice { get; set; } = 0;
        public string Description { get; set; } = "";
        public string ReturnPolicy { get; set; } = "";
        public Guid MainCategoryId { get; set; } = Guid.NewGuid();
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public Guid SubcategoryId { get; set; } = Guid.NewGuid();
        public string ImageContainerId { get; set; } = "";
        public string Season { get; set; } = "";
        public List<Guid> LTCs { get; set; } = new List<Guid>();
    }
}
