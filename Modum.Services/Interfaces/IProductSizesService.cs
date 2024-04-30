using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Services.Interfaces
{
    public interface IProductSizesService : IBaseService<ProductSizesHelpingTable>
    {
        Task<List<ProductSizesHelpingTable>> GetSizesByProductId(Guid productId);
        Task<Product> GetMostBoughtProducts();
        Task<ProductSizesHelpingTable> GetSizeBasedOnItsName(Guid productId, string name);
        Task<List<ProductSizesHelpingTable>> GetSizesOfProducts(List<Product> products);
    }
}
