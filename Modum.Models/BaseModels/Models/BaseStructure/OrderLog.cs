using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class OrderLog : IEntity
    {
        public int Id { get; set; } = 0;

        public string PublicOrderId { get; set; } = "";

        public int UserId { get; set; } = 0;

        public OrderStatus ProductStatus { get; set; } = OrderStatus.NoInformation;

        public string DescriptionIfNeeded { get; set; } = "";

        public List<Product> Products { get; set; } = new List<Product>();

        public int ProductOwner { get; set; } = 0;

        public string Country { get; set; } = "";

        public string Province { get; set; } = "";

        public string Municipality { get; set; } = "";

        public string City { get; set; } = "";

        public string Street { get; set; } = "";

        public string NumberOfStreet { get; set; } = "";

        public string ExternalDescription { get; set; } = "";

        //The closest delivery algo
        public string SecondaryAddress { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string SecondaryPhoneNumber { get; set; } = "";

        public string EmailAddress { get; set; } = "";

        public string DeliveryBy { get; set; } = "";

        public DateTime OrderRegistered { get; set; } = DateTime.Now;

        public int BillOfLading { get; set; } = 0;

        public int InvoiceId { get; set; } = 0;

        public int AvailableReturnPeriodDays { get; set; } = 0;

        public string RequiredDocumentsAfterArrival { get; set; } = "";
    }
}
