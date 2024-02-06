using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.LTCs;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.ProductStructure
{
    public class Product : IEntity
    {
        [Required]
        public int Id { get; set; } = 0;
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Brand { get; set; } = "";
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public string UploadedBy { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        public string ReturnPolicy { get; set; } = "";
        public List<Product> SimilarProducts { get; set; } = new List<Product>();
        public List<Product> LastLookedProducts { get; set; } = new List<Product>();
        public int MainCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public int AmountOfTimesInFavourites { get; set; } = 0;
        public decimal DiscountFromPrice { get; set; } = 0;
        public string ImageContainerId { get; set; } = "";
        public IEnumerable<ProductSizesHelpingTable> ProductSizes { get; set; } = new List<ProductSizesHelpingTable>();
        public string Colour { get; set; } = "";
        public DateTime UploadedOrUpdatedOn { get; set; } = DateTime.Now;
        public string Season { get; set; } = "All Seasons";
        public List<LTC> LTCs { get; set; } = new List<LTC>();
    }
}
