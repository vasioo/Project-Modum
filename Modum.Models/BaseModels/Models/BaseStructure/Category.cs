using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Category : IEntity
    {
        public Category()
        {
            Subcategories = new List<Subcategory>();
        }


        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string CreatorName { get; set; } = "";

        [Required]
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        [Required]
        public Guid MainCategoryId { get; set; }

        public bool IsSelectedForNav { get; set; } = false;
    }
}
