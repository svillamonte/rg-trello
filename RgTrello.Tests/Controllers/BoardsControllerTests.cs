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

        private readonly BoardsController _boardsController;

        public BoardsControllerTests()
        {
            _mockTrelloService = new Mock<ITrelloService>();

            _boardsController = new BoardsController(_mockTrelloService.Object);
        }

        [TestMethod]
        public void Index_WithNoExceptions_MethodCallsTokenManagerAndTrelloService()
        {
            // Act
            var result = _boardsController.Index() as ViewResult;

            // Assert
            _mockTrelloService.Verify(x => x.GetBoards(), Times.Once);
        }

        [TestMethod]
        public void Index_WithApiReturningThreeBoards_ViewResultIsBuiltWithRelatedBoardModels()
        {
            // Arrange
            var boardOne = new TrelloBoard { Id = "aaa111", Name = "Board one" };
            var boardTwo = new TrelloBoard { Id = "aaa222", Name = "Board two" };
            var boardThree = new TrelloBoard { Id = "aaa333", Name = "Board three" };

            _mockTrelloService
                .Setup(x => x.GetBoards())
                .Returns(new[] { boardOne, boardTwo, boardThree });

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

        [TestMethod]
        public void Board_WithNoExceptions_MethodCallsTokenManagerAndTrelloService()
        {
            // Act
            var boardId = "aniceboardid";

            var result = _boardsController.Board(boardId) as ViewResult;

            // Assert
            _mockTrelloService.Verify(x => x.GetBoardCards(boardId), Times.Once);
        }

        [TestMethod]
        public void Board_WithApiReturningThreeCards_ViewResultIsBuiltWithRelatedCardModels()
        {
            // Arrange
            var boardId = "aniceboardid";

            var cardOne = new TrelloCard { Id = "aaa111", Name = "Card one", Description = "Description one" };
            var cardTwo = new TrelloCard { Id = "aaa222", Name = "Card two", Description = "Description one" };
            var cardThree = new TrelloCard { Id = "aaa333", Name = "Card three", Description = "Description one" };

            _mockTrelloService
                .Setup(x => x.GetBoardCards(boardId))
                .Returns(new[] { cardOne, cardTwo, cardThree });

            // Act
            var result = _boardsController.Board(boardId) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<CardModel>));
            var enumerable = (IEnumerable<CardModel>)result.Model;

            Assert.AreEqual(cardOne.Id, enumerable.ElementAt(0).Id);
            Assert.AreEqual(cardOne.Name, enumerable.ElementAt(0).Name);

            Assert.AreEqual(cardTwo.Id, enumerable.ElementAt(1).Id);
            Assert.AreEqual(cardTwo.Name, enumerable.ElementAt(1).Name);

            Assert.AreEqual(cardThree.Id, enumerable.ElementAt(2).Id);
            Assert.AreEqual(cardThree.Name, enumerable.ElementAt(2).Name);
        }

        [TestMethod]
        public void Board_WithApiThrowingException_RedirectsToHomeController()
        {
            // Arrange
            var boardId = "aniceboardid";

            _mockTrelloService.Setup(x => x.GetBoardCards(boardId)).Throws(new TokenNotFoundException());

            // Act
            var result = _boardsController.Board(boardId) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }
    }
}
