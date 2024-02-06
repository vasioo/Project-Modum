using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class MainCategoryService : BaseService<MainCategory>, IMainCategoryService
    {
        private readonly DataContext _dataContext;
        private readonly ICategoryService _categoryService;
        public MainCategoryService(DataContext context, ICategoryService categoryService) : base(context)
        {
            _dataContext = context;
            _categoryService = categoryService;
        }

        public Task<MainCategory> GetDefaultMainCategory()
        {
            var mainCategory = _dataContext.Set<MainCategory>().OrderBy(x => x.Id).First();
            return Task.FromResult(mainCategory);
        }

        public async Task<bool> SaveCategoriesAndSubcategoriesAsync(List<Category> categories)
        {
            try
            {
                var mainCategoryId = categories.First().MainCategoryId;
                var itemsToRemove = await _categoryService.GetCategoriesByMainCategoryAsync(mainCategoryId);

                await _categoryService.RemoveRangeAsync(itemsToRemove);

                foreach (var category in categories)
                {
                    await _categoryService.AddAsync(category);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
