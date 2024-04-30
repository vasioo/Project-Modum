using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.PaymentStructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modum.Models.BaseModels.Models.Payment
{
    public class Cart : IEntity
    {
        [Key]
        public Guid Id { get; set; } 

        [ForeignKey("User")]
        public string UserId { get; set; } = "";

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
