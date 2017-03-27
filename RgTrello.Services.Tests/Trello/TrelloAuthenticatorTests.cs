using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RgTrello.Services.Trello;

namespace RgTrello.Services.Tests.Trello
{
    [TestClass]
    public class TrelloAuthenticatorTests
    {
        [TestMethod]
        public void Authenticate_CallsRestRequestWithConstructorParametersValues()
        {
            // Arrange
            var applicationKey = "aniceapplicationkey";
            var userToken = "aniceusertoken";

            var authenticator = new TrelloAuthenticator(applicationKey, userToken);

            var _mockRestClient = new Mock<IRestClient>();
            var _mockRestRequest = new Mock<IRestRequest>();

            // Act
            authenticator.Authenticate(_mockRestClient.Object, _mockRestRequest.Object);

            // Assert
            _mockRestRequest.Verify(x => x.AddParameter("key", applicationKey), Times.Once);
            _mockRestRequest.Verify(x => x.AddParameter("token", userToken), Times.Once);
        }
    }
}
