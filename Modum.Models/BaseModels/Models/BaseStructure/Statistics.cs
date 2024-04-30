using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Statistics : IEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime TimeOfStatistic { get; set; } = DateTime.Now;

        [Required]
        [NotMapped]
        public BigInteger AmountOfUsers { get; set; } = 0;

        [Required]
        [NotMapped]

        public BigInteger AmountOfOrders { get; set; } = 0;

        [Required]
        public decimal TotalSpenditureOfOrders { get; set; } = 0;

        [Required]
        [NotMapped]

        public BigInteger AmountOfMen { get; set; } = 0;

        [Required]
        [NotMapped]

        public BigInteger AmountOfWomen { get; set; } = 0;

        [Required]
        public decimal SpenditureOfWomen { get; set; } = 0;

        [Required]
        public decimal SpenditureOfMen { get; set; } = 0;


    }
}
