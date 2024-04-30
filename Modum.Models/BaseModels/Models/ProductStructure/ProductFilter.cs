namespace Modum.Models.BaseModels.Models.ProductStructure
{
    public class ProductFilter
    {
        public string MainCategoryName { get; set; } = "Women";
        public Guid CategoryId { get; set; } 
        public string SelectedSubcategories { get; set; } = "";
        public string ProductColours { get; set; } = "";
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = 0;
        public string MadeFrom { get; set; } = "";
        public string Print { get; set; } = "";
        public string LengthOfSleeve { get; set; } = "";
        public string Cut { get; set; } = "";
        public string Style { get; set; } = "";
        public string Sizes { get; set; } = "";
        public string Model { get; set; } = "";
        public string Fastening { get; set; } = "";
        public string TypeOfProduct { get; set; } = "";
        public string Sport { get; set; } = "";
        public string Season { get; set; } = "";
        public string Brands { get; set; } = "";
        public string SearchString { get; set; } = "";
        public string LTCs { get; set; } = "";
    }
}

