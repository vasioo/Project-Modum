using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services.ControllerService.DocsController;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Tests.UnitTests.ControllerTests.DocsControllerFolder
{
    public class DocsControllerHelperTests
    {
        private readonly Mock<IDocsService> docsServiceMock;
        private readonly Mock<IFirebaseService> firebaseServiceMock;

        public DocsControllerHelperTests()
        {
            docsServiceMock = new Mock<IDocsService>();
            firebaseServiceMock = new Mock<IFirebaseService>();
        }

        [Fact]
        public async Task SaveDocInformation_ValidData_ReturnsTrue()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var doc = new BlogPost { /* set your properties */ };
            var fileName = "testFileName";
            firebaseServiceMock.Setup(x => x.AddABlogPost(doc)).ReturnsAsync(Guid.NewGuid());
            docsServiceMock.Setup(x => x.SaveImage(It.IsAny<Photo>())).ReturnsAsync(true);

            // Act
            var result = await controllerHelper.SaveDocInformation(doc, fileName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetAllDocuments_ReturnsQueryableBlogPosts()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var blogPosts = new List<BlogPost> { /* create your test data */ };
            firebaseServiceMock.Setup(x => x.GetAllBlogPosts()).Returns(blogPosts.AsQueryable());

            // Act
            var result = controllerHelper.GetAllDocuments();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IQueryable<BlogPost>>(result);
        }

        [Fact]
        public async Task EditDocHelper_ValidId_ReturnsBlogPost()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var id = Guid.NewGuid();
            var blogPost = new BlogPost { /* create your test data */ };
            firebaseServiceMock.Setup(x => x.GetBlogPostById(id)).ReturnsAsync(blogPost);

            // Act
            var result = await controllerHelper.EditDocHelper(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BlogPost>(result);
        }

        [Fact]
        public async Task EditDocPostHelper_ValidData_ReturnsTrue()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var doc = new BlogPost { /* set your properties */ };
            var blogImage = "testBlogImage";
            var id = Guid.NewGuid();
            firebaseServiceMock.Setup(x => x.UpdateABlogPostAsync(doc.Id, blogImage)).ReturnsAsync(id);
            firebaseServiceMock.Setup(x => x.DeleteImage(blogImage)).ReturnsAsync(true);
            docsServiceMock.Setup(x => x.SaveImage(It.IsAny<Photo>())).ReturnsAsync(true);

            // Act
            var result = await controllerHelper.EditDocPostHelper(doc, blogImage);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteDocPost_ValidId_ReturnsEmptyString()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var id = Guid.NewGuid();
            var blogPost = new BlogPost { Id = id, /* set your properties */ };
            firebaseServiceMock.Setup(x => x.GetBlogPostById(id)).ReturnsAsync(blogPost);
            firebaseServiceMock.Setup(x => x.RemoveABlogPostAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await controllerHelper.DeleteDocPost(id);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public async Task DeleteDocPost_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var controllerHelper = new DocsControllerHelper(docsServiceMock.Object, firebaseServiceMock.Object);

            var id = Guid.NewGuid();
            firebaseServiceMock.Setup(x => x.GetBlogPostById(id)).ReturnsAsync((BlogPost)null);

            // Act
            var result = await controllerHelper.DeleteDocPost(id);

            // Assert
            Assert.Equal("The entity is not present in the database", result);
        }
    }
}
