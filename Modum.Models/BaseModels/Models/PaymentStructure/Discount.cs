using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.PaymentStructure
{
    public class Discount : IEntity
    {
        [Key]
        public Guid Id { get; set; } 

        public string DiscountKey { get; set; } = "";

        public double DiscountPercentile { get; set; } = 0;

        public string CoveredMainCategory { get; set; } = "";
        public string CoveredCategory { get; set; } = "";
        public string CoveredSubcategory { get; set; } = "";
    }
}
