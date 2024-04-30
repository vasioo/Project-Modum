using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.MainModel;

namespace Modum.Models.BaseModels.Models.PaymentStructure
{
    public class Order:IEntity
    {
        public Guid Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
        public string DeliveryLocation { get; set; } = "";
        public string TypeOfDelivery { get; set; } = "";
        public DateTime DateOfOrdering { get; set; }
        public decimal PricePaid { get; set; }
        public List<ProductSizesHelpingTable> Products { get; set; } = new List<ProductSizesHelpingTable>();
        public string StripeReceiptURL { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string StripeId { get; set; } = "";
    }
}
