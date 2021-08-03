namespace MiniFootball.Test.Services
{
    using Data.Models;
    using MiniFootball.Services.Admins;
    using Mocks;
    using Xunit;

    public class AdminServiceTest
    {
        private const string UserId = "TestUserId";

        [Fact]
        public void IsAdminShouldReturnTrueWhenUserIsAdmin()
        {
            // Arrange
            var adminService = GetAdminService();

            // Act
            var result = adminService.IsAdmin(UserId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdminShouldReturnFalseWhenUSerIsNotAdmin()
        {
            // Arrange
            var adminService = GetAdminService();

            // Act
            var result = adminService.IsAdmin("AnotherUserId");

            // Assert
            Assert.False(result);
        }

        private static IAdminService GetAdminService()
        {
            var data = DatabaseMock.Instance;

            data.Admins.Add(new Admin { UserId = UserId });
            data.SaveChanges();

            return new AdminService(data);
        }

    }
}
