using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Services;

namespace Modum.Services.Interfaces
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly DataContext dataContext;
        public CategoryService(DataContext context) : base(context)
        {
            dataContext = context;
        }

        public Task<IEnumerable<Category>> GetCategoriesByMainCategoryAsync(Guid mainCategoryId)
        {
            var categories = dataContext.Set<Category>().Where(category => category.MainCategoryId == mainCategoryId);
            return Task.FromResult(categories.AsEnumerable());
        }
    }
}
