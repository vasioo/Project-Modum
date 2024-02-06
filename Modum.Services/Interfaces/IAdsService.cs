using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Services.Interfaces
{
    public interface IAdsService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProductsByTenMostAddedToFavourites();
        Task<IEnumerable<Product>> GetProductsByTenMostBought();
    }
}
