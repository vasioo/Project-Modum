using Modum.DataAccess.MainModel;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class BannedUserServiceTests : ServiceTestsBase
    {
        private IBannedUsersService bannedUserService;

        public BannedUserServiceTests()
        {
            bannedUserService = new BannedUsersService(this.context);
        }
        [Fact]
        public async Task GetUserById_ReturnsCorrectResult()
        {
            var result = await bannedUserService.GetUserByUserId("1");

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetUserByNullId_ReturnsNull()
        {
            var result = await bannedUserService.GetUserByUserId(null);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByNonexistentId_ReturnsNull()
        {
            var result = await bannedUserService.GetUserByUserId("NonexistentId");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByEmptyId_ReturnsNull()
        {
            var result = await bannedUserService.GetUserByUserId(string.Empty);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByInvalidId_ReturnsNull()
        {
            var result = await bannedUserService.GetUserByUserId("InvalidId");

            Assert.Null(result);
        }
    }
}