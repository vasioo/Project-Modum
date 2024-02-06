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
        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string CreatorName { get; set; } = "";

        [Required]
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        [Required]
        public int MainCategoryId { get; set; } = 0;

        public bool IsSelectedForNav { get; set; } = false;
    }
}
