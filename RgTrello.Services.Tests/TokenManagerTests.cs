using Microsoft.VisualStudio.TestTools.UnitTesting;
using RgTrello.Services.Exceptions;

namespace RgTrello.Services.Tests
{
    [TestClass]
    public class TokenManagerTests
    {
        [TestMethod]
        public void AddUserToken_ForKeyNoPresentYet_AddsKeyValuePairContainingToken()
        {
            // Arrange
            var token = "anicetesttoken";

            var tokenManager = new TokenManager();

            // Act
            tokenManager.AddUserToken(token);

            // Assert
            Assert.AreEqual(token, tokenManager.GetUserToken());
        }

        [TestMethod]
        public void AddUserToken_ForKeyAlreadyPresent_DoesNothing()
        {
            // Arrange
            var token = "anicetesttoken";

            var tokenManager = new TokenManager();
            tokenManager.AddUserToken(token);

            // Act
            tokenManager.AddUserToken(token);

            // Assert
            Assert.AreEqual(token, tokenManager.GetUserToken());
        }

        [TestMethod]
        public void GetUserToken_ForKeyAlreadyPresent_ReturnsUserToken()
        {
            // Arrange
            var token = "anicetesttoken";

            var tokenManager = new TokenManager();
            tokenManager.AddUserToken(token);

            // Act
            var result = tokenManager.GetUserToken();

            // Assert
            Assert.AreEqual(token, result);
        }

        [TestMethod]
        [ExpectedException(typeof(TokenNotFoundException))]
        public void GetUserToken_ForKeyNotPresentYet_ThrowsTokenNotFoundException()
        {
            // Arrange
            var token = "anicetesttoken";

            var tokenManager = new TokenManager();

            // Act
            var result = tokenManager.GetUserToken();
        }
    }
}
