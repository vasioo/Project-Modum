using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.PaymentStructure
{
    public class Invoice : IEntity
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; } = "";
        public DateTime DateOfCreation { get; set; }
        public int InvoiceNumber { get; set; } = 0;
        public string Barcode { get; set; } = "";
        public string ReceiverName { get; set; } = "";
        public string CompanyIdentificationNumber { get; set; } = "";
        public string Address { get; set; } = "";

        public string Country { get; set; } = "";
        public string BankBillNumber { get; set; } = "";
        public string BankName { get; set; } = "";
        public string ClientNumber { get; set; } = "";

        public string Description { get; set; } = "";
        public string MeasuringUnit { get; set; } = "";
        public string Quantity { get; set; } = "";
        public string PriceForSingleItem { get; set; } = "";
        public string PriceForAllItems { get; set; } = "";

        public string TaxBracket { get; set; } = "";
        public decimal TaxBracketPercentile { get; set; } = 0;
        public decimal TaxBracketAmount { get; set; } = 0;
        public string TotalSum { get; set; } = "";
    }
}
