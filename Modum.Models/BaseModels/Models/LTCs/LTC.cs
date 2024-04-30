using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.LTCs
{
    public class LTC : IEntity
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
