using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Brands : IEntity
    {
        public int Id { get; set; }

        public string BrandName { get; set; } = "";
        public string TypeOfBrand { get; set; } = "";
        public decimal AmountOfTimesInFavourites { get; set; } = 0;
    }
}
