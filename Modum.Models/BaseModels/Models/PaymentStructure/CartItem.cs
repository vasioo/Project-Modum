using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.PaymentStructure
{
    public class CartItem : IEntity
    {
        public Guid Id { get; set; } 
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; } = "";
    }
}
