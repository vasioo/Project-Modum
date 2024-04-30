using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Services.Interfaces
{
    public interface IProductService : IBaseService<Product>
    {
        Task<bool> SaveImages(List<Photo> images);
        Task<IEnumerable<Product>> GetProductsByMainCategoryAsync(Guid mainCategoryId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid caetgoryId);
        Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(Guid subcategoryId);
        Task<IEnumerable<Product>> GetProductsByTitleAsync(string title);
        Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal price);
        Task<IEnumerable<Product>> GetProductsByWhoUploadedThemAsync(string username);
        Task<IEnumerable<Product>> GetProductsByAvailableSizesAsync(IEnumerable<string> availableSizes);
        Task<IEnumerable<Product>> GetProductsByTenMostAddedToFavourites();
        Task<IEnumerable<Product>> GetProductsByTenMostBought();
        Task<IQueryable<Product>> GetProductsByFiltersAsync(ProductFilter filter);
        Task<List<string>> GetMostFavouriteBrandsBySoldItemsBySection(string section);
        Task DailyCheckForLTC();
        Task<bool> SaveImage(Photo image);
    }
}
