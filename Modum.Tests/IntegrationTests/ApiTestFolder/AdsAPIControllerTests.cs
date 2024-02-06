using Microsoft.AspNetCore.Mvc;
using Modum.AdsWebApi.Controllers;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Tests.IntegrationTests.ApiTestFolder
{
    public class AdsAPIControllerTests
    {
        [Fact]
        public async Task GetMostAddedToFavAdData_ReturnsContentResult_WithValidData()
        {
            // Arrange
            var mockAdsService = new Mock<IAdsService>();
            mockAdsService.Setup(s => s.GetProductsByTenMostAddedToFavourites())
                .ReturnsAsync(new List<Product> { });

            var controller = new AdsApiController(mockAdsService.Object);

            // Act
            var result = await controller.GetMostAddedToFavAdData();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("text/html", contentResult.ContentType);
        }

        [Fact]
        public async Task GetMostBoughtAdData_ReturnsContentResult_WithValidData()
        {
            // Arrange
            var mockAdsService = new Mock<IAdsService>();
            mockAdsService.Setup(s => s.GetProductsByTenMostBought())
                .ReturnsAsync(new List<Product> { });

            var controller = new AdsApiController(mockAdsService.Object);

            // Act
            var result = await controller.GetMostBoughtAdData();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("text/html", contentResult.ContentType);
        }
    }
}
