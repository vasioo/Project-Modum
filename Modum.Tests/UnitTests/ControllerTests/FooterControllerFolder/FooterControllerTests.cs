using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modum.Models.MainModel;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Modum.Web.Controllers;
using Modum.Web.ControllerService.FooterController;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Modum.Tests.UnitTests.ControllerTests.FooterControllerFolder
{
    public class FooterControllerTests
    {
        private readonly Mock<IFooterControllerHelper> controllerHelperMock;
        private readonly Mock<IFirebaseService> firebaseServiceMock;
        private readonly Mock<IConfiguration> configurationServiceMock;
        private readonly Mock<IEmailSenderService> emailServiceMock;
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly FooterController controller;
        private DefaultHttpContext httpContext;

        public FooterControllerTests()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);
            controllerHelperMock = new Mock<IFooterControllerHelper>();
            firebaseServiceMock = new Mock<IFirebaseService>();
            configurationServiceMock = new Mock<IConfiguration>();
            emailServiceMock = new Mock<IEmailSenderService>();
            httpContext = new DefaultHttpContext();
            controller = new FooterController(controllerHelperMock.Object, emailServiceMock.Object,
                                            firebaseServiceMock.Object, configurationServiceMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        private void SetupUser(string username)
        {
            var user = new ApplicationUser { UserName = username };

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) }));

            userManagerMock.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);
        }

        #region Views
        [Fact]
        public async Task AboutUs_ReturnsViewResult()
        {
            //Arrange
            var username = "testuser";
            SetupUser(username);
            controllerHelperMock.Setup(x => x.GetAmountOfCartItemsForUser(null)).ReturnsAsync(0);

            // Act
            var result = await controller.AboutUs() as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/FooterItems/AboutUs.cshtml", ((ViewResult)result).ViewName);
        }

        [Fact]
        public async Task Ads_ReturnsViewResult()
        {
            //Arrange
            var username = "testuser";
            SetupUser(username);
            controllerHelperMock.Setup(x => x.GetAmountOfCartItemsForUser(null)).ReturnsAsync(0);

            // Act
            var result = await controller.Ads();

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/FooterItems/Ads.cshtml", ((ViewResult)result).ViewName);
        }

        [Fact]
        public async Task Campaigns_ReturnsViewResult()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);
            var viewModel = new List<LTC>();
            controllerHelperMock.Setup(x => x.GetCampaignInformationData()).ReturnsAsync(viewModel);

            // Act
            var result = await controller.Campaigns();

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/FooterItems/Campaigns.cshtml", ((ViewResult)result).ViewName);
        }

        #endregion

        #region Blogs

        [Fact]
        public async Task Blog_ReturnsViewResult()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);
            var blogPosts = new List<BlogPost> { /* create your test data */ };
            firebaseServiceMock.Setup(x => x.GetAllBlogPosts()).Returns(blogPosts.AsQueryable());

            // Act
            var result = await controller.Blog() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/FooterItems/Blog.cshtml", result.ViewName);
            Assert.IsAssignableFrom<BlogViewModel>(result.Model);
        }

        [Fact]
        public async Task BlogPost_ValidPostId_ReturnsViewResult()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);
            var postId = Guid.NewGuid();
            var blogPost = new BlogPost { };
            firebaseServiceMock.Setup(x => x.GetBlogPostById(postId)).ReturnsAsync(blogPost);

            // Act
            var result = await controller.BlogPost(postId);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/FooterItems/BlogPost.cshtml", ((ViewResult)result).ViewName);
            Assert.IsAssignableFrom<BlogPostViewModel>(((ViewResult)result).Model);
        }

        [Fact]
        public async Task BlogPost_InvalidPostId_ReturnsViewResult()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);
            var postId = Guid.NewGuid();
            firebaseServiceMock.Setup(x => x.GetBlogPostById(postId)).ReturnsAsync((BlogPost)null);
            firebaseServiceMock.Setup(x => x.GetAllBlogPosts()).Returns(new List<BlogPost>().AsQueryable());

            // Act
            var result = await controller.BlogPost(postId) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/FooterItems/Blog.cshtml", result.ViewName);
        }

        #endregion

        #region Emails

        [Fact]
        public async Task UserSendEmail_Successful_ReturnsJsonResult()
        {
            // Arrange
            var email = "test@test.com";
            var bodyText = "Test body text";
            var name = "John Doe";
            var status = HttpStatusCode.OK;

            configurationServiceMock.Setup(x => x[It.IsAny<string>()]).Returns("testConfigValue");
            emailServiceMock.Setup(x => x.ReceiveEmail(email, bodyText, "user-message", name)).Returns(status);

            // Act
            var result = await controller.UserSendEmail(email, bodyText, name) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value!)!);
        }

        [Fact]
        public async Task UserSendEmail_Error_ReturnsJsonResult()
        {
            // Arrange
            var email = "test@test.com";
            var bodyText = "Test body text";
            var name = "John Doe";
            var status = HttpStatusCode.InternalServerError;

            configurationServiceMock.Setup(x => x[It.IsAny<string>()]).Returns("testConfigValue");
            emailServiceMock.Setup(x => x.ReceiveEmail(email, bodyText, "user-message", name)).Returns(status);

            // Act
            var result = await controller.UserSendEmail(email, bodyText, name) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value!)!);
        }

        [Fact]
        public async Task PartnershipSendEmail_Successful_ReturnsJsonResult()
        {
            // Arrange
            var email = "test@test.com";
            var bodyText = "Test body text";
            var name = "John Doe";
            var status = HttpStatusCode.OK;

            configurationServiceMock.Setup(x => x[It.IsAny<string>()]).Returns("testConfigValue");
            emailServiceMock.Setup(x => x.ReceiveEmail(email, bodyText, "partnerships", name)).Returns(status);

            // Act
            var result = await controller.PartnershipSendEmail(email, bodyText, name) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value!)!);
        }

        [Fact]
        public async Task PartnershipSendEmail_Error_ReturnsJsonResult()
        {
            // Arrange
            var email = "test@test.com";
            var bodyText = "Test body text";
            var name = "John Doe";
            var status = HttpStatusCode.InternalServerError;

            configurationServiceMock.Setup(x => x[It.IsAny<string>()]).Returns("testConfigValue");
            emailServiceMock.Setup(x => x.ReceiveEmail(email, bodyText, "partnerships", name)).Returns(status);

            // Act
            var result = await controller.PartnershipSendEmail(email, bodyText, name) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value!)!);
        }

        #endregion
    }
}
