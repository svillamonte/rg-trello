using Microsoft.VisualStudio.TestTools.UnitTesting;
using RgTrello.Services.Trello;

namespace RgTrello.Services.Tests.Trello
{
    [TestClass]
    public class TrelloApiClientTests
    {
        [TestMethod]
        public void SetToken_AssignsInstanceOfIAuthenticator()
        {
            // Arrange
            var token = "anicetoken";

            var clientWithoutAuthenticator = new TrelloApiClient();
            var clientWithAuthenticator = new TrelloApiClient();

            // Act
            clientWithAuthenticator.SetToken(token);

            // Assert
            Assert.IsNull(clientWithoutAuthenticator.Authenticator);
            Assert.IsNotNull(clientWithAuthenticator.Authenticator);
        }
    }
}
