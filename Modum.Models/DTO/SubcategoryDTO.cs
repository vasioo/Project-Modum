namespace Modum.Web.Models.Models.DTO
{
    public class SubcategoryDTO
    {
        public string SubcategoryName { get; set; } = "";
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public Guid MainCategoryId { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; } = "";
    }
}
