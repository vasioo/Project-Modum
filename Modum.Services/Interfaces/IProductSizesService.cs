using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Services.Interfaces
{
    public interface IProductSizesService : IBaseService<ProductSizesHelpingTable>
    {
        Task<IEnumerable<ProductSizesHelpingTable>> GetSizesByProductId(int productId);
    }
}
