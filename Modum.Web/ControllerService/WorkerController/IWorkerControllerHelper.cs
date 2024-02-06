using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.DTO;
using Modum.Models.ViewModels;
using Modum.Web.Models.Models.DTO;

namespace Modum.Services.Services.ControllerService.WorkerController
{
    public interface IWorkerControllerHelper
    {
        Task<AddProductViewModel> AddProductHelper();
        Task AddProductJSONHelper(ProductDTO productDTO, List<ImageDTO> imagesDTO, string username);
        Task<EditProductViewModel> EditProductHelper(int productId);
        Task EditProductJSONHelper(ProductDTO productDTO, List<ImageDTO> imagesDTO, string username);
        Task DeleteProductHelper(int productId);
        Task<IEnumerable<MainCategory>> GetMainCategoriesAsyncHelper();
        Task<bool> ManageSubSelectionHelper(int mainCategoryId, List<CategoryDTO> categoriesDTO, List<SubcategoryDTO> subcategoriesDTO, string username);
        IQueryable<Product> ManageProductsJSON();
        IQueryable<BlogPost> GetBlogsFromDatabase();
        Task<bool> AddABlogPostToFirebase(BlogPost post, List<ImageDTO> imagesDTO);
        Task<BlogPost> GetBlogById(Guid blogId);
        IQueryable<LTC> ManageLTCHelper();
        Task AddLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO);
        Task<LTC> EditLTCHelper(int ltcId);
        Task EditLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO);
        Task DeleteLTCHelper(int ltcId);
        Task<MainCategory> LoadMainCategoryHelper(int mainCategoryId);
        Task<object> FilterMainCategoryDataHelper(int mainCategoryId, int categoryId);
        Task<int> GetCategoryBySubcategoryAsyncHelper(int subcategoryId);
    }
}
