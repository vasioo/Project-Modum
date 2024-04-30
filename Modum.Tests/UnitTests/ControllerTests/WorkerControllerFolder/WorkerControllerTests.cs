using Microsoft.AspNetCore.Mvc;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.DTO;
using Modum.Models.ViewModels;
using Modum.Services.Services.ControllerService.WorkerController;
using Modum.Web.Controllers;
using Modum.Web.Models.Models.DTO;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.WorkerControllerFolder
{
    public class WorkerControllerTests
    {
        private Mock<IWorkerControllerHelper> controllerHelperMock;
        private WorkerController controller;

        public WorkerControllerTests()
        {
            controllerHelperMock = new Mock<IWorkerControllerHelper>();
            controller = new WorkerController(helper: controllerHelperMock.Object);
        }

        #region ManageBlogs
        [Fact]
        public void AddABlog_ReturnsViewResult()
        {
            // Act
            var result = controller.AddABlog() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/AddABlog.cshtml", result.ViewName);
        }

        [Fact]
        public async Task AddAPostAction_RedirectsToAction()
        {
            // Arrange
            var model = new BlogPost();
            var imagesDTO = new List<ImageDTO>();

            // Act
            var result = await controller.AddAPostAction(model, imagesDTO) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AddABlog", result.ActionName);
            Assert.Equal("Worker", result.ControllerName);
        }

        [Fact]
        public async Task EditBlog_ReturnsViewResult()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            controllerHelperMock.Setup(x => x.GetBlogById(blogId)).ReturnsAsync(new BlogPost());

            // Act
            var result = await controller.EditBlog(blogId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/EditBlogPost.cshtml", result.ViewName);
        }
        #endregion

        #region ManageSubSelection
        [Fact]
        public async Task ManageSubSelection_ReturnsViewResult()
        {
            // Arrange
            var mainCategories = new List<MainCategory>();
            controllerHelperMock.Setup(x => x.GetMainCategoriesAsyncHelper()).ReturnsAsync(mainCategories);

            // Act
            var result = await controller.ManageSubSelection() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/ManageSubSelection.cshtml", result.ViewName);
        }

        [Fact]
        public async Task ManageSubSelectionPost_ReturnsJsonResult()
        {
            // Arrange
            var mainCategoryId = Guid.NewGuid();
            var categoriesDTO = new List<CategoryDTO>();
            var subcategoriesDTO = new List<SubcategoryDTO>();

            // Act
            var result = await controller.ManageSubSelection(mainCategoryId, categoriesDTO, subcategoriesDTO) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }
        #endregion

        #region ManageProducts
        [Fact]
        public async Task ManageProducts_ReturnsViewResult()
        {
            // Act
            var result = await controller.ManageProducts(null, null, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/ManageProducts.cshtml", result.ViewName);
        }

        [Fact]
        public async Task AddProduct_ReturnsViewResult()
        {
            // Act
            var result = await controller.AddProduct() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/AddProduct.cshtml", result.ViewName);
        }

        [Fact]
        public async Task EditProduct_ReturnsViewResult()
        {
            var productId = Guid.NewGuid();
            controllerHelperMock.Setup(x => x.EditProductHelper(productId)).ReturnsAsync(new EditProductViewModel());

            // Act
            var result = await controller.EditProduct(productId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/EditProduct.cshtml", result.ViewName);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsJsonResult()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var result = await controller.DeleteProduct(productId) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }
        #endregion

        #region ManageLimitedTimeCampaigns
        [Fact]
        public async Task ManageLTCs_ReturnsViewResult()
        {
            // Act
            var result = await controller.ManageLTCs(null, null, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/ManageLTCs.cshtml", result.ViewName);
        }

        [Fact]
        public async Task AddALTC_ReturnsViewResult()
        {
            // Act
            var result = await controller.AddALTC() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/AddALTC.cshtml", result.ViewName);
        }

        [Fact]
        public async Task EditLTC_ReturnsViewResult()
        {
            var ltcId = Guid.NewGuid();
            controllerHelperMock.Setup(x => x.EditLTCHelper(ltcId)).ReturnsAsync(new LTC());

            // Act
            var result = await controller.EditLTC(ltcId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Worker/EditALTC.cshtml", result.ViewName);
        }

        [Fact]
        public async Task DeleteLTC_ReturnsJsonResult()
        {
            //Arrange
            var ltcId = Guid.NewGuid();

            // Act
            var result = await controller.DeleteLTC(ltcId) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }
        #endregion
    }
}
