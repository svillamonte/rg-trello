using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.AspNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RgTrello.Auth;
using RgTrello.Controllers;
using RgTrello.Services.Interfaces;

namespace RgTrello.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        private readonly Mock<ITokenManager> _mockTokenManager;
        private readonly Mock<IOAuthWebSecurity> _mockOAuthWebSecurity;

        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTests()
        {
            _mockTokenManager = new Mock<ITokenManager>();
            _mockOAuthWebSecurity = new Mock<IOAuthWebSecurity>();
            
            _authenticationController = new AuthenticationController(_mockTokenManager.Object, _mockOAuthWebSecurity.Object);
            MockUrlHelper();
        }

        [TestMethod]
        public void Index_WithNormalFlow_ReturnsViewButIsNotSeenAnyway()
        {
            // Arrange
            _mockOAuthWebSecurity
                .Setup(x => x.RequestAuthentication("Trello", It.IsAny<string>()));

            // Act
            var result = _authenticationController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void OAuthCallBack_WithSuccessfulResult_RedirectsToBoardsController()
        {
            // Arrange
            var extraData = new Dictionary<string, string>
            {
                {"accesstoken", "anicetesttoken" }
            };

            var authenticationResult = new AuthenticationResult(true, "Trello", "userId", "userName", extraData);

            _mockOAuthWebSecurity
                .Setup(x => x.VerifyAuthentication(It.IsAny<string>()))
                .Returns(authenticationResult);

            // Act
            var result = _authenticationController.OAuthCallBack(It.IsAny<string>()) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Boards", result.RouteValues["controller"]);
            _mockTokenManager.Verify(x => x.AddUserToken(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OAuthCallBack_WithNotSuccessfulResult_RedirectsToHomeController()
        {
            // Arrange
            var authenticationResult = new AuthenticationResult(false, "Trello", "userId", "userName", null);

            _mockOAuthWebSecurity
                .Setup(x => x.VerifyAuthentication(It.IsAny<string>()))
                .Returns(authenticationResult);

            // Act
            var result = _authenticationController.OAuthCallBack(It.IsAny<string>()) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            _mockTokenManager.Verify(x => x.AddUserToken(It.IsAny<string>()), Times.Never);
        }

        private void MockUrlHelper()
        {
            var routes = new RouteCollection();

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(x => x.ApplyAppPathModifier("/post1")).Returns("http://localhost/post1");

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);
            
            _authenticationController.ControllerContext = new ControllerContext(context.Object, new RouteData(), _authenticationController);
            _authenticationController.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);
        }
    }
}
