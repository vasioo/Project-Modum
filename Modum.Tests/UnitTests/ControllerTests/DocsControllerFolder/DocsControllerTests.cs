using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.Docs;
using Modum.Models.ViewModels;
using Modum.Services.Services.ControllerService.DocsController;
using Modum.Web.Controllers;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.DocsControllerFolder
{
    public class DocsControllerTests
    {
        private Mock<IDocsControllerHelper> controllerHelperMock;
        private readonly DocsController controller;

        public DocsControllerTests()
        {
            controllerHelperMock = new Mock<IDocsControllerHelper>();
            controller = new DocsController(controllerHelperMock.Object);
        }
        #region DocsShower
        [Fact]
        public void DocsShower_ReturnsView()
        {
            // Arrange
            controllerHelperMock.Setup(x => x.GetAllDocuments()).Returns(new List<BlogPost>().AsQueryable());

            var controller = new DocsController(controllerHelperMock.Object)
            {
            };

            // Act
            var result = controller.DocsShower() as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Docs/DocsShower.cshtml", viewResult.ViewName);
        }
        #endregion

        #region CreateDocs
        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var controller = new DocsController(controllerHelperMock.Object)
            {
            };

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Docs/CreateDocument.cshtml", viewResult.ViewName);
        }

        [Fact]
        public async Task CreatePost_ValidModel_ReturnsViewResult()
        {
            // Arrange
            var helperMock = new Mock<IDocsControllerHelper>();
            var controller = new DocsController(helperMock.Object);

            var doc = new Doc { Title = "Test Title", Content = "Test Content" };
            var formFileMock = new Mock<IFormFile>();

            helperMock.Setup(x => x.SaveDocInformation(It.IsAny<BlogPost>(), It.IsAny<string>()));

            // Act
            var result = await controller.CreatePost(doc, formFileMock.Object) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/Docs/CreateDocument.cshtml", result.ViewName);
        }
        #endregion

        #region EditDocs
        [Fact]
        public async Task EditDocument_ValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new DocsController(controllerHelperMock.Object);

            var id = Guid.NewGuid();
            var expectedDoc = new BlogPost { Id = id, Title = "Test Title", Content = "Test Content" };

            controllerHelperMock.Setup(x => x.EditDocHelper(id)).ReturnsAsync(expectedDoc);

            // Act
            var result = await controller.EditDocument(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/Docs/EditDocument.cshtml", result.ViewName);
            Assert.Equal(expectedDoc, result.Model);
        }
        #endregion

        #region DeleteDocs
        [Fact]
        public async Task DeleteDocument_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange

            var invalidId = Guid.NewGuid();
            var blogPosts = new List<BlogPost>().AsQueryable();

            controllerHelperMock.Setup(x => x.GetAllDocuments()).Returns(blogPosts);

            // Act
            var result = await controller.DeleteDocument(invalidId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteDocumentPost_ValidId_ReturnsJsonResult()
        {
            // Arrange
            var controller = new DocsController(controllerHelperMock.Object);

            var id = Guid.NewGuid();

            // Act
            var result = await controller.DeleteDocumentPost(id);

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }

        [Fact]
        public async Task DeleteDocumentPost_InvalidId_ReturnsJsonResultWithStatusFalse()
        {
            // Arrange
            var controller = new DocsController(controllerHelperMock.Object);

            var invalidId = Guid.NewGuid();
            controllerHelperMock.Setup(x => x.DeleteDocPost(invalidId)).ThrowsAsync(new Exception());

            // Act
            var result = await controller.DeleteDocumentPost(invalidId);

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }
        #endregion

    }
}
