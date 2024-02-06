using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Services;

namespace Modum.Services.Interfaces
{
    public class SubcategoryService : BaseService<Subcategory>, ISubcategoryService
    {
        private readonly DataContext dataContext;
        public SubcategoryService(DataContext context) : base(context)
        {
            dataContext = context;
        }
        public Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryNameAsync(string categoryName)
        {
            var subcategories = dataContext.Set<Subcategory>().Where(sub => sub.CategoryName == categoryName);
            return Task.FromResult(subcategories.AsEnumerable());
        }
    }
}
