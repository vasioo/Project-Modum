using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.PaymentStructure;
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
        Task<EditProductViewModel> EditProductHelper(Guid productId);
        Task EditProductJSONHelper(ProductDTO productDTO, List<ImageDTO> imagesDTO, string username);
        Task DeleteProductHelper(Guid productId);
        Task<IEnumerable<MainCategory>> GetMainCategoriesAsyncHelper();
        Task<bool> ManageSubSelectionHelper(Guid mainCategoryId, List<CategoryDTO> categoriesDTO, List<SubcategoryDTO> subcategoriesDTO, string username);
        IQueryable<ProductSizesHelpingTable> ManageProductsJSON();
        IQueryable<BlogPost> GetBlogsFromDatabase();
        Task<bool> AddABlogPostToFirebase(BlogPost post, List<ImageDTO> imagesDTO);
        Task<BlogPost> GetBlogById(Guid blogId);
        IQueryable<LTC> ManageLTCHelper();
        Task AddLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO);
        Task<LTC> EditLTCHelper(Guid ltcId);
        Task EditLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO);
        Task DeleteLTCHelper(Guid ltcId);
        Task<MainCategory> LoadMainCategoryHelper(Guid mainCategoryId);
        Task<object> FilterMainCategoryDataHelper(Guid mainCategoryId, Guid categoryId);
        Task<Guid> GetCategoryBySubcategoryAsyncHelper(Guid subcategoryId);
        Task<bool> DeleteCategoryOrSubcategory(bool isCategory, Guid id);
        IQueryable<Order> GetAllOrdersFromCategory(string category);
        Task<bool> ChangeDeliveryStatusHelper(Guid orderId, string newStatus);
    }
}
