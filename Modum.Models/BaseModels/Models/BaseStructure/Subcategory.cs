using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Subcategory : IEntity
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string CreatorName { get; set; } = "";

        public string CategoryName { get; set; } = "";
        public Category Category { get; set; } = new Category();
        public int CategoryId { get; set; } = 0;
        public bool IsSelectedForNav { get; set; } = false;
        public int MainCategoryId { get; set; } = 0;

    }
}
