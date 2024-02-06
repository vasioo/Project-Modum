using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface IMainCategoryService : IBaseService<MainCategory>
    {
        Task<bool> SaveCategoriesAndSubcategoriesAsync(List<Category> categories);
        Task<MainCategory> GetDefaultMainCategory();
    }
}
