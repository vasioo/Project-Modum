using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Subcategory : IEntity
    {
        [Required]
        public Guid Id { get; set; } 

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string CreatorName { get; set; } = "";

        public string CategoryName { get; set; } = "";
        public Category Category { get; set; } = new Category();
        public bool IsSelectedForNav { get; set; } = false;
        public Guid MainCategoryId { get; set; }

    }
}
