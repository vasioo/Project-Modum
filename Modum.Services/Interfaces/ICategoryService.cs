using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesByMainCategoryAsync(Guid mainCategoryId);
    }
}
