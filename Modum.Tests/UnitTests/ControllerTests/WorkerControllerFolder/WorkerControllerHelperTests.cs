using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services.ControllerService.WorkerController;
using Modum.Web.Models.Models.DTO;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.WorkerControllerFolder
{
    public class WorkerControllerHelperTests
    {
        private readonly Mock<IProductService> productServiceMock;
        private readonly Mock<ILTCService> ltcServiceMock;
        private readonly Mock<IMainCategoryService> mainCategoryServiceMock;
        private readonly Mock<ISubcategoryService> subcategoryServiceMock;
        private readonly Mock<ICategoryService> categoryServiceMock;
        private readonly Mock<IProductSizesService> productSizesServiceMock;
        private readonly Mock<IFirebaseService> firebaseServiceMock;
        private readonly Mock<IOrderService> orderServiceMock;
        private readonly WorkerControllerHelper helper;

        public WorkerControllerHelperTests()
        {
            productServiceMock = new Mock<IProductService>();
            ltcServiceMock = new Mock<ILTCService>();
            mainCategoryServiceMock = new Mock<IMainCategoryService>();
            subcategoryServiceMock = new Mock<ISubcategoryService>();
            categoryServiceMock = new Mock<ICategoryService>();
            productSizesServiceMock = new Mock<IProductSizesService>();
            firebaseServiceMock = new Mock<IFirebaseService>();
            orderServiceMock = new Mock<IOrderService>();

            helper = new WorkerControllerHelper(
                mainCategoryServiceMock.Object,
                subcategoryServiceMock.Object,
                categoryServiceMock.Object,
                productServiceMock.Object,
                productSizesServiceMock.Object,
                firebaseServiceMock.Object,
                ltcServiceMock.Object,
                orderServiceMock.Object
            );
        }

        #region ManageBlogs
        [Fact]
        public void GetBlogsFromDatabase_ReturnsQueryableBlogPosts()
        {
            // Arrange
            var blogPosts = new List<BlogPost>().AsQueryable();
            firebaseServiceMock.Setup(x => x.GetAllBlogPosts()).Returns(blogPosts);

            // Act
            var result = helper.GetBlogsFromDatabase();

            // Assert
            Assert.Equal(blogPosts, result);
        }

        [Fact]
        public async Task AddABlogPostToFirebase_Success_ReturnsTrue()
        {
            // Arrange
            var post = new BlogPost();
            var imagesDTO = new List<ImageDTO> { new ImageDTO { Image = "image1" } };
            productServiceMock.Setup(x => x.SaveImages(It.IsAny<List<Photo>>())).ReturnsAsync(true);

            // Act
            var result = await helper.AddABlogPostToFirebase(post, imagesDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddABlogPostToFirebase_Failure_ReturnsFalse()
        {
            // Arrange
            var post = new BlogPost();
            var imagesDTO = new List<ImageDTO> { new ImageDTO { Image = "image1" } };
            productServiceMock.Setup(x => x.SaveImages(It.IsAny<List<Photo>>())).Throws(new Exception("Test exception"));

            // Act
            var result = await helper.AddABlogPostToFirebase(post, imagesDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetBlogById_ReturnsBlogPost()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var blogPost = new BlogPost();
            firebaseServiceMock.Setup(x => x.GetBlogPostById(blogId)).ReturnsAsync(blogPost);

            // Act
            var result = await helper.GetBlogById(blogId);

            // Assert
            Assert.Equal(blogPost, result);
        }

        #endregion

        #region ManageProducts
        [Fact]
        public async Task AddProductHelper_ReturnsAddProductViewModel()
        {
            // Arrange
            var mainCategories = new List<MainCategory>().AsQueryable();
            var categories = new List<Category>().AsQueryable();
            var subcategories = new List<Subcategory>().AsQueryable();
            var sizes = Enum.GetValues(typeof(ClothesSizes));
            var ltcList = new List<LTC>().AsQueryable();

            mainCategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mainCategories);
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(categories);
            subcategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(subcategories);
            ltcServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(ltcList);

            // Act
            var result = await helper.AddProductHelper();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mainCategories, result.MainCategoryList);
            Assert.NotNull(result.CategoryList);
            Assert.NotNull(result.SubcategoryList);
            Assert.Equal(sizes, result.Sizes);
            Assert.Equal(ltcList, result.LTCs);
        }

        [Fact]
        public async Task AddProductJSONHelper_Success()
        {
            // Arrange
            var productDTO = new ProductDTO();
            var imagesDTO = new List<ImageDTO> { new ImageDTO { Image = "image1" } };
            var username = "testuser";

            productServiceMock.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(Guid.NewGuid());
            productServiceMock.Setup(x => x.SaveImages(It.IsAny<List<Photo>>())).ReturnsAsync(true);

            // Act
            await helper.AddProductJSONHelper(productDTO, imagesDTO, username);

            // Assert
            productServiceMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            productServiceMock.Verify(x => x.SaveImages(It.IsAny<List<Photo>>()), Times.Once);
        }

        [Fact]
        public async Task EditProductHelper_ReturnsEditProductViewModel()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mainCategories = new List<MainCategory>().AsQueryable();
            var categories = new List<Category>().AsQueryable();
            var subcategories = new List<Subcategory>().AsQueryable();
            var product = new Product();
            var sizes = Enum.GetValues(typeof(ClothesSizes));
            var ltcList = new List<LTC>().AsQueryable();

            mainCategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mainCategories);
            subcategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(subcategories);
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(categories);
            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            ltcServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(ltcList);

            // Act
            var result = await helper.EditProductHelper(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mainCategories, result.MainCategoryList);
            Assert.Equal(ltcList, result.LTCs);
            Assert.NotNull(result.CategoryList);
            Assert.NotNull(result.SubcategoryList);
            Assert.Equal(product, result.Product.FirstOrDefault().Product);
            Assert.Equal(product.ImageContainerId, result.CloudinaryImageContainerId);
            Assert.Equal(sizes, result.Sizes);
        }

        [Fact]
        public async Task EditProductJSONHelper_Success()
        {
            // Arrange
            var productDTO = new ProductDTO();
            var imagesDTO = new List<ImageDTO> { new ImageDTO { Image = "image1" } };
            var username = "testuser";

            productSizesServiceMock.Setup(x => x.UpdateAsync(It.IsAny<ProductSizesHelpingTable>())).ReturnsAsync(1);
            productServiceMock.Setup(x => x.SaveImages(It.IsAny<List<Photo>>())).ReturnsAsync(true);
            productSizesServiceMock.Setup(x => x.UpdateRangeAsync(It.IsAny<List<ProductSizesHelpingTable>>())).ReturnsAsync(1);

            // Act
            await helper.EditProductJSONHelper(productDTO, imagesDTO, username);

            // Assert
            productServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
            productServiceMock.Verify(x => x.SaveImages(It.IsAny<List<Photo>>()), Times.Once);
            productSizesServiceMock.Verify(x => x.UpdateRangeAsync(It.IsAny<List<ProductSizesHelpingTable>>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductHelper_Success()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product();

            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            await helper.DeleteProductHelper(productId);

            // Assert
            productServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetMainCategoriesAsyncHelper_ReturnsMainCategories()
        {
            // Arrange
            var mainCategories = new List<MainCategory>().AsQueryable();
            mainCategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mainCategories);

            // Act
            var result = await helper.GetMainCategoriesAsyncHelper();

            // Assert
            Assert.Equal(mainCategories, result);
        }

        #endregion

        #region ManageSubSelection


        #endregion

        #region ManageLimitedTimeCampaigns


        #endregion
    }
}
