using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface ISubcategoryService : IBaseService<Subcategory>
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryNameAsync(string categoryName);

    }
}
