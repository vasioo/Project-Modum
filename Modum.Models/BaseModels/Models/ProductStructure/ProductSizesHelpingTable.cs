using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.ProductStructure
{
    public class ProductSizesHelpingTable : IEntity
    {
        public int Id { get; set; } = 0;
        public string ProductSize { get; set; } = "";
        public int AvailableItems { get; set; } = 0;
        public int AllTimeAvailableItems { get; set; } = 0;

        public Product Product { get; set; } = new Product();
    }
}
