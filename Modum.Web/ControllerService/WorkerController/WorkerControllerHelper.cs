using Microsoft.AspNetCore.Mvc.Rendering;
using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.DTO;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Modum.Web.Models.Models.DTO;
using System.Text;

namespace Modum.Services.Services.ControllerService.WorkerController
{
    public class WorkerControllerHelper : IWorkerControllerHelper
    {
        #region FieldsHelper
        private readonly IProductService _productService;
        private readonly ILTCService _ltcService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IProductSizesService _productSizesService;
        private readonly IFirebaseService _firebaseService;
        private readonly IOrderService _orderService;
        #endregion

        #region ConstructorHelper
        public WorkerControllerHelper(IMainCategoryService mainCategoryService,
            ISubcategoryService subcategoryService, ICategoryService categoryService,
            IProductService productService, IProductSizesService productSizesService,
            IFirebaseService firebaseService, ILTCService ltcService,
            IOrderService orderService)
        {
            _mainCategoryService = mainCategoryService;
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
            _productService = productService;
            _productSizesService = productSizesService;
            _firebaseService = firebaseService;
            _ltcService = ltcService;
            _orderService = orderService;
        }
        #endregion

        #region ManageBlogsHelper

        public IQueryable<BlogPost> GetBlogsFromDatabase()
        {
            var data = _firebaseService.GetAllBlogPosts();
            return data;
        }

        public async Task<bool> AddABlogPostToFirebase(BlogPost post, List<ImageDTO> imagesDTO)
        {
            try
            {
                await _firebaseService.AddABlogPost(post);

                var images = new List<Photo>();

                int counter = 1;

                foreach (var image in imagesDTO)
                {
                    var photo = new Photo();
                    if (image.Image != null && image.Image != "")
                    {
                        photo.Image = image.Image;
                        photo.ImageName = $"image-{counter}-for-blog-{post.Id}";
                        photo.PublicId = $"image-{counter}-for-blog-{post.Id}";
                        images.Add(photo);
                        counter++;
                    }
                }

                await _productService.SaveImages(images);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<BlogPost> GetBlogById(Guid blogId)
        {
            var data = await _firebaseService.GetBlogPostById(blogId);

            return data;
        }

        #endregion

        #region ManageProductsHelper
        public async Task<AddProductViewModel> AddProductHelper()
        {
            var mainCategories = await _mainCategoryService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var subcategories = await _subcategoryService.GetAllAsync();
            var sizes = Enum.GetValues(typeof(ClothesSizes));
            SelectList categoryList = new SelectList(categories.Where(cat => cat.MainCategoryId == mainCategories.FirstOrDefault().Id), "Id", "Name");
            SelectList subcategoryList = new SelectList(subcategories.Where(sub => sub.MainCategoryId == mainCategories.FirstOrDefault().Id), "Id", "Name");

            var addPrViewModel = new AddProductViewModel();

            addPrViewModel.MainCategoryList = mainCategories;
            addPrViewModel.CategoryList = categoryList;
            addPrViewModel.SubcategoryList = subcategoryList;
            addPrViewModel.Sizes = sizes;
            addPrViewModel.LTCs = await _ltcService.GetAllAsync();

            return addPrViewModel;
        }

        public async Task AddProductJSONHelper(ProductDTO productDTO, List<ImageDTO> imagesDTO, string username)
        {
            Random random = new Random();
            string randomId = GenerateRandomId(8, random);

            var productSizes = new List<ProductSizesHelpingTable>();

            foreach (var size in productDTO.Sizes)
            {
                var item = new ProductSizesHelpingTable();

                if (Enum.TryParse(size.Split('-')[0], out ClothesSizes prodSize))
                {
                    item.ProductSize = prodSize.ToString();
                    item.AvailableItems = int.Parse(size.Split('-')[1]);
                    item.AllTimeAvailableItems = item.AvailableItems;
                    productSizes.Add(item);
                }
            }
            await _productSizesService.AddRangeAsync(productSizes);

            var product = new Product();

            product.ImageContainerId = randomId;
            product.Title = productDTO.Title;
            product.Brand = productDTO.Brand;
            product.Price = productDTO.Price;
            product.DiscountFromPrice = productDTO.DiscountFromPrice;
            product.Description = productDTO.Description;
            product.ReturnPolicy = productDTO.ReturnPolicy;
            product.MainCategoryId = _categoryService.IQueryableGetAllAsync()
                    .Where(x => x.Id == productDTO.CategoryId).Select(x => x.MainCategoryId).FirstOrDefault();
            product.CategoryId = productDTO.CategoryId;
            product.SubcategoryId = productDTO.SubcategoryId;
            product.UploadedBy = username;
            product.Colour = productDTO.Colour;
            product.Season = productDTO.Season;
            var ltcs = new List<LTC>();
            foreach (var item in productDTO.LTCs)
            {
                var neededItem = await _ltcService.GetByIdAsync(item);
                ltcs.Add(neededItem);
            }
            product.LTCs = ltcs;

            var images = new List<Photo>();

            int counter = 1;
            foreach (var image in imagesDTO)
            {
                var photo = new Photo();
                if (image.Image != null && image.Image != "")
                {
                    photo.Image = image.Image;
                    photo.ImageName = $"image-{counter}-for-{randomId}";
                    photo.PublicId = $"image-{counter}-for-{randomId}";
                    images.Add(photo);
                    counter++;
                }
            }


            await _productService.AddAsync(product);

            await _productService.SaveImages(images);
        }

        public async Task<EditProductViewModel> EditProductHelper(Guid productId)
        {
            var editPrVModel = new EditProductViewModel();
            var mainCategories = await _mainCategoryService.GetAllAsync();
            var subcategories = await _subcategoryService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            var productSizes =await _productSizesService.GetSizesByProductId(productId);
            var product = productSizes.FirstOrDefault().Product;

            SelectList categoryList = new SelectList(categories.Where(cat => cat.MainCategoryId == product.MainCategoryId), "Id", "Name");
            SelectList subcategoryList = new SelectList(subcategories.Where(sub => sub.MainCategoryId == product.MainCategoryId), "Id", "Name");

            var sizes = Enum.GetValues(typeof(ClothesSizes));

            editPrVModel.MainCategoryList = mainCategories;
            editPrVModel.LTCs = await _ltcService.GetAllAsync();
            editPrVModel.CategoryList = categoryList;
            editPrVModel.SubcategoryList = subcategoryList;
            editPrVModel.Product = productSizes;
            editPrVModel.CloudinaryImageContainerId = product.ImageContainerId;
            editPrVModel.Sizes = sizes;
            return editPrVModel;
        }

        public async Task EditProductJSONHelper(ProductDTO productDTO, List<ImageDTO> imagesDTO, string username)
        {
            var productSizes = new List<ProductSizesHelpingTable>();

            foreach (var size in productDTO.Sizes)
            {
                var item = new ProductSizesHelpingTable();

                //this would throw errors
                item.Id = Guid.Parse(size.Split('-')[2]);
                item.ProductSize = size.Split('-')[0];
                item.AvailableItems = int.Parse(size.Split('-')[1]);
                item.AllTimeAvailableItems = int.Parse(size.Split('-')[3]);
                productSizes.Add(item);
            }

            var product = new Product();
            product.Id = productDTO.Id;
            product.Title = productDTO.Title;
            product.Brand = productDTO.Brand;
            product.Price = productDTO.Price;
            product.DiscountFromPrice = productDTO.DiscountFromPrice;
            product.Description = productDTO.Description;
            product.ReturnPolicy = productDTO.ReturnPolicy;
            product.MainCategoryId = _categoryService.IQueryableGetAllAsync().Where(x => x.Id == productDTO.CategoryId).Select(x => x.MainCategoryId).FirstOrDefault();
            product.CategoryId = productDTO.CategoryId;
            product.SubcategoryId = productDTO.SubcategoryId;
            product.UploadedBy = username;
            product.ImageContainerId = productDTO.ImageContainerId;
            product.Colour = productDTO.Colour;
            var ltcs = new List<LTC>();
            foreach (var item in productDTO.LTCs)
            {
                var neededItem = await _ltcService.GetByIdAsync(item);
                ltcs.Add(neededItem);
            }
            product.LTCs = ltcs;

            var images = new List<Photo>();

            int counter = 1;
            foreach (var image in imagesDTO)
            {
                var photo = new Photo();
                if (image.Image != null && image.Image != "")
                {
                    photo.Image = image.Image;
                    photo.ImageName = $"image-{counter}-for-{productDTO.ImageContainerId}";
                    images.Add(photo);
                }
                counter++;
            }

            await _productService.SaveImages(images);

            await _productSizesService.UpdateRangeAsync(productSizes);

            await _productService.UpdateAsync(product);
        }

        public async Task DeleteProductHelper(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product != null)
            {
                await _productService.RemoveAsync(product.Id);
            }
        }

        public async Task<IEnumerable<MainCategory>> GetMainCategoriesAsyncHelper()
        {
            return await _mainCategoryService.GetAllAsync();
        }
        public IQueryable<ProductSizesHelpingTable> ManageProductsJSON()
        {
            return _productSizesService.IQueryableGetAllAsync();
        }

        #endregion

        #region ManageSubSelectionHelper

        public async Task<bool> ManageSubSelectionHelper(Guid mainCategoryId, List<CategoryDTO> categoriesDTO, List<SubcategoryDTO> subcategoriesDTO, string username)
        {
            var categories = new List<Category>();
            foreach (var categoryDTO in categoriesDTO)
            {
                var category = new Category()
                {
                    Name = categoryDTO.CategoryName,
                    MainCategoryId = mainCategoryId,
                    CreatorName = username
                };

                foreach (var subcategoryDTO in subcategoriesDTO)
                {
                    var subcategory = new Subcategory();

                    subcategory.Name = subcategoryDTO.SubcategoryName;
                    subcategory.CategoryName = subcategoryDTO.CategoryName;
                    subcategory.CreatorName = username;
                    subcategory.MainCategoryId = mainCategoryId;

                    if (subcategory.CategoryName == category.Name)
                    {
                        category.Subcategories.Add(subcategory);
                    }
                }

                categories.Add(category);
            }

            return await _mainCategoryService.SaveCategoriesAndSubcategoriesAsync(categories);
        }

        public async Task<bool> DeleteCategoryOrSubcategory(bool isCategory, Guid id)
        {
            try
            {
                if (isCategory)
                {
                    await _categoryService.RemoveAsync(id);
                }
                else
                {
                    await _subcategoryService.RemoveAsync(id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region HelperMethodsHelper
        public async Task<MainCategory> LoadMainCategoryHelper(Guid mainCategoryId)
        {
            var mainCategoryAssociatedData = await _mainCategoryService.GetByIdAsync(mainCategoryId);
            var categoryAssociatedData = await _categoryService.GetCategoriesByMainCategoryAsync(mainCategoryId);

            var categories = new List<Category>();

            if (categoryAssociatedData != null)
            {
                foreach (var category in categoryAssociatedData)
                {
                    var tempCategoryModel = new Category();
                    tempCategoryModel.Id = category.Id;
                    tempCategoryModel.Name = category.Name;
                    if (category.Subcategories != null)
                    {
                        var subcategories = new List<Subcategory>();
                        foreach (var subcategory in category.Subcategories)
                        {
                            var tempSubcategoryModel = new Subcategory();
                            tempSubcategoryModel.Id = subcategory.Id;
                            tempSubcategoryModel.Name = subcategory.Name;

                            subcategories.Add(tempSubcategoryModel);
                        }
                        tempCategoryModel.Subcategories = subcategories;
                    }
                    categories.Add(tempCategoryModel);
                }

            }
            var model = new MainCategory();
            model.Name = mainCategoryAssociatedData.Name;
            model.Id = mainCategoryAssociatedData.Id;
            model.Categories = categories;
            return model;
        }

        public async Task<object> FilterMainCategoryDataHelper(Guid mainCategoryId, Guid categoryId)
        {
            if (mainCategoryId != Guid.Empty)
            {
                var categories = await _categoryService.GetAllAsync();
                categories = categories.Where(cat => cat.MainCategoryId == mainCategoryId);

                var subcategories = await _subcategoryService.GetAllAsync();
                subcategories = subcategories.Where(sub => sub.MainCategoryId == mainCategoryId);

                if (categoryId != Guid.Empty)
                {
                    var category = await _categoryService.GetByIdAsync(categoryId);

                    if (category != null)
                    {
                        subcategories = subcategories.Where(sub => sub.MainCategoryId == mainCategoryId && sub.Category.Id == category.Id);

                        var subcategoriesToReturn = subcategories.Select(sub => new
                        {
                            sub.Id,
                            sub.Name
                        }).ToList();

                        return subcategoriesToReturn;
                    }
                }
                else
                {
                    var categoriesToReturn = categories.Select(cat => new
                    {
                        cat.Id,
                        cat.Name
                    }).ToList();

                    var subcategoriesToReturn = subcategories.Select(cat => new
                    {
                        cat.Id,
                        cat.Name
                    }).ToList();

                    var dataToReturn = new
                    {
                        Categories = categoriesToReturn,
                        Subcategories = subcategoriesToReturn
                    };

                    return dataToReturn;
                }
            }

            return null;
        }

        public async Task<Guid> GetCategoryBySubcategoryAsyncHelper(Guid subcategoryId)
        {
            var subcategory = await _subcategoryService.GetByIdAsync(subcategoryId);

            var category = await _categoryService.GetByIdAsync(subcategory.Category.Id);

            return category.Id;
        }

        #endregion

        #endregion

        #region ManageLimitedTimeCampaignsHelper
        public IQueryable<LTC> ManageLTCHelper()
        {
            return _ltcService.IQueryableGetAllAsync();
        }

        public async Task AddLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO)
        {
            var ltc = new LTC();

            ltc.StartDate = ltcDTO.StartDate;
            ltc.EndDate = ltcDTO.EndDate;
            ltc.Content = ltcDTO.Content;
            ltc.Description = ltcDTO.Description;
            ltc.Title = ltcDTO.Title;
            ltc.PercentageOfDiscount = ltcDTO.PercentageOfDiscount;
            var id = await _ltcService.AddAsync(ltc);

            var photo = new Photo();
            if (imageDTO.Image != null && imageDTO.Image != "")
            {
                photo.Image = imageDTO.Image;
                photo.ImageName = $"image-for-ltc-{id}";
                photo.PublicId = $"image-for-ltc-{id}";
            }

            await _ltcService.SaveImage(photo);
        }

        public async Task<LTC> EditLTCHelper(Guid ltcId)
        {
            var ltc = await _ltcService.GetByIdAsync(ltcId);
            return ltc;
        }

        public async Task EditLTCJSONHelper(LTCDTO ltcDTO, ImageDTO imageDTO)
        {
            var ltc = new LTC();

            ltc.StartDate = ltcDTO.StartDate;
            ltc.EndDate = ltcDTO.EndDate;
            ltc.Content = ltcDTO.Content;
            ltc.Title = ltcDTO.Title;
            ltc.PercentageOfDiscount = ltcDTO.PercentageOfDiscount;
            var photo = new Photo();
            if (imageDTO.Image != null && imageDTO.Image != "")
            {
                photo.Image = imageDTO.Image;
                photo.ImageName = $"image-for-ltc-{ltcDTO.Id}";
                photo.PublicId = $"image-for-ltc-{ltcDTO.Id}";
            }

            await _ltcService.SaveImage(photo);

            await _ltcService.UpdateAsync(ltc);
        }

        public async Task DeleteLTCHelper(Guid ltcId)
        {
            var ltc = await _ltcService.GetByIdAsync(ltcId);

            if (ltc != null)
            {
                await _ltcService.RemoveAsync(ltc.Id);
            }
        }

        #endregion

        #region ManageOrdersHelper
        public IQueryable<Order> GetAllOrdersFromCategory(string category)
        {
            return _orderService.IQueryableGetAllAsync().Where(x=>x.OrderStatus==category);
        }

        public async Task<bool> ChangeDeliveryStatusHelper(Guid orderId, string newStatus)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(orderId);
                order.OrderStatus = newStatus;

                await _orderService.UpdateAsync(order);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        #endregion

        #region Helpers
        static string GenerateRandomId(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder idBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                idBuilder.Append(chars[index]);
            }

            return idBuilder.ToString();
        }
        #endregion
    }
}
