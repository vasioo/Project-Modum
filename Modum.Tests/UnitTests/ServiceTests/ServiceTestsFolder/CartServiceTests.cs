using Modum.Models.BaseModels.Models.Payment;
using Modum.Services.Interfaces;
using Modum.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class CartServiceTests : ServiceTestsBase
    {
        private ICartService cartService;

        public CartServiceTests()
        {
            cartService = new CartService(context);
        }
        [Fact]
        public async Task GetCartContainerByUserId_ReturnsCorrectCart()
        {
            // Arrange
            var userId = "TestUser";
            var cartToAdd = new Cart { UserId = userId };
            await context.Cart.AddAsync(cartToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await cartService.GetCartContainerByUserId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetCartContainerByUserId_ReturnsNullForNonexistentUser()
        {
            // Arrange
            var nonExistentUserId = "NonExistentUser";

            // Act
            var result = await cartService.GetCartContainerByUserId(nonExistentUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCartIdByUserId_ReturnsCorrectCartId()
        {
            // Arrange
            var userId = "TestUser";
            var cartToAdd = new Cart { UserId = userId };
            await context.Cart.AddAsync(cartToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await cartService.GetCartIdByUserId(userId);

            // Assert
            Assert.NotEqual(0, result);
            Assert.Equal(cartToAdd.Id, result);
        }

        [Fact]
        public async Task GetCartIdByUserId_ReturnsZeroForNonexistentUser()
        {
            // Arrange
            var nonExistentUserId = "NonExistentUser";

            // Act
            var result = await cartService.GetCartIdByUserId(nonExistentUserId);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
