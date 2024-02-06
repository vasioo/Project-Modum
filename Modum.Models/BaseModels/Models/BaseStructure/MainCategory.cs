using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class MainCategory : IEntity
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
