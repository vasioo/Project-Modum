using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.PaymentStructure
{
    public class CartItem : IEntity
    {
        public int Id { get; set; } = 0;
        public int CartId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public string Size { get; set; } = "";
    }
}
