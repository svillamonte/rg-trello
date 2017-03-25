using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RgTrello.Controllers;
using RgTrello.Models.Boards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Tests.Controllers
{
    [TestClass]
    public class BoardsControllerTests
    {
        private readonly Mock<ITrelloService> _mockTrelloService;
        private readonly Mock<ITokenManager> _mockTokenManager;

        private readonly BoardsController _boardsController;

        public BoardsControllerTests()
        {
            _mockTrelloService = new Mock<ITrelloService>();
            _mockTokenManager = new Mock<ITokenManager>();
            _mockTokenManager.Setup(x => x.GetUserToken()).Returns("anicetokentotest");

            _boardsController = new BoardsController(_mockTrelloService.Object, _mockTokenManager.Object);
        }

        [TestMethod]
        public void Index_WithNoExceptions_MethodCallsTokenManagerAndTrelloService()
        {
            // Act
            var result = _boardsController.Index() as ViewResult;

            // Assert
            _mockTokenManager.Verify(x => x.GetUserToken(), Times.Once);
            _mockTrelloService.Verify(x => x.SetToken(_mockTokenManager.Object.GetUserToken()), Times.Once);
            _mockTrelloService.Verify(x => x.GetBoards(), Times.Once);
        }

        [TestMethod]
        public void Index_WithApiReturningThreeBoards_ViewResultIsBuiltWithRelatedBoardModels()
        {
            // Arrange
            var boardOne = new TrelloBoard { Id = "aaa111", Name = "Board one" };
            var boardTwo = new TrelloBoard { Id = "aaa222", Name = "Board two" };
            var boardThree = new TrelloBoard { Id = "aaa333", Name = "Board three" };

            _mockTrelloService.Setup(x => x.GetBoards()).Returns(new[] { boardOne, boardTwo, boardThree });

            // Act
            var result = _boardsController.Index() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<BoardModel>));
            var enumerable = (IEnumerable<BoardModel>)result.Model;

            Assert.AreEqual(boardOne.Id, enumerable.ElementAt(0).Id);
            Assert.AreEqual(boardOne.Name, enumerable.ElementAt(0).Name);

            Assert.AreEqual(boardTwo.Id, enumerable.ElementAt(1).Id);
            Assert.AreEqual(boardTwo.Name, enumerable.ElementAt(1).Name);

            Assert.AreEqual(boardThree.Id, enumerable.ElementAt(2).Id);
            Assert.AreEqual(boardThree.Name, enumerable.ElementAt(2).Name);
        }

        [TestMethod]
        public void Index_WithApiThrowingException_RedirectsToHomeController()
        {
            // Arrange
            _mockTrelloService.Setup(x => x.GetBoards()).Throws(new TokenNotFoundException());

            // Act
            var result = _boardsController.Index() as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }
    }
}
