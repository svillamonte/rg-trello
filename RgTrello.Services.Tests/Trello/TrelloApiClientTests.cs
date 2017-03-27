using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RgTrello.Services.Trello;

namespace RgTrello.Services.Tests.Trello
{
    [TestClass]
    public class TrelloApiClientTests
    {
        [TestMethod]
        public void Execute_WithoutGenerics_AssignsInstanceOfIAuthenticator()
        {
            // Arrange
            var token = "anicetoken";

            var mockRestRequest = new Mock<IRestRequest>();

            var clientWithAuthenticator = new TrelloApiClient();

            // Act
            var result = clientWithAuthenticator.Execute(mockRestRequest.Object, token);

            // Assert
            Assert.IsNotNull(clientWithAuthenticator.Authenticator);
        }
    }
}
