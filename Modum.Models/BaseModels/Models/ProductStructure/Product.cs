using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.LTCs;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.ProductStructure
{
    public class Product : IEntity
    {
        [Required]
        public Guid Id { get; set; }
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
        public Guid MainCategoryId { get; set; } 
        public Guid CategoryId { get; set; } 
        public Guid SubcategoryId { get; set; }
        public int AmountOfTimesInFavourites { get; set; } = 0;
        public decimal DiscountFromPrice { get; set; } = 0;
        public string ImageContainerId { get; set; } = "";
        public string Colour { get; set; } = "";
        public DateTime UploadedOrUpdatedOn { get; set; } = DateTime.Now;
        public string Season { get; set; } = "All Seasons";
        public List<LTC> LTCs { get; set; } = new List<LTC>();
    }
}
