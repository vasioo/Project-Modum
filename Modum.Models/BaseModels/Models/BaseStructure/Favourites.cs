using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.ProductStructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Favourites : IEntity
    {
        [Key]
        public Guid Id { get; set; } 

        [ForeignKey("User")]
        public string UserId { get; set; } = "";


        public List<Product> Products { get; set; } = new List<Product>();
    }
}
