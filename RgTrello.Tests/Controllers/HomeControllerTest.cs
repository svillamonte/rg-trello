using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RgTrello.Controllers;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private readonly Mock<ITokenManager> _mockTokenManager;

        private readonly HomeController _homeController;

        public HomeControllerTest()
        {
            _mockTokenManager = new Mock<ITokenManager>();
            
            _homeController = new HomeController(_mockTokenManager.Object);
        }

        [TestMethod]
        public void Index_ForNormalFlow_RedirectsToBoardsController()
        {
            // Arrange
            _mockTokenManager
                .Setup(x => x.GetUserToken())
                .Returns("anicetokentotest");

            // Act
            var result = _homeController.Index() as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Boards", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void Index_WithTokenManagerThrowingException_ShowsView()
        {
            // Arrange
            _mockTokenManager
                .Setup(x => x.GetUserToken())
                .Throws(new TokenNotFoundException());

            // Act
            var result = _homeController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
