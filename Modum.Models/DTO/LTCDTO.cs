namespace Modum.Models.DTO
{
    public class LTCDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PercentageOfDiscount { get; set; }
    }
}
