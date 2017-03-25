﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RgTrello.Controllers;
using RgTrello.Models.Cards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Tests.Controllers
{
    [TestClass]
    public class CardsControllerTests
    {
        private readonly Mock<ITrelloService> _mockTrelloService;
        private readonly Mock<ITokenManager> _mockTokenManager;

        private readonly CardsController _cardsController;

        public CardsControllerTests()
        {
            _mockTrelloService = new Mock<ITrelloService>();
            _mockTokenManager = new Mock<ITokenManager>();
            _mockTokenManager.Setup(x => x.GetUserToken()).Returns("anicetokentotest");

            _cardsController = new CardsController(_mockTrelloService.Object, _mockTokenManager.Object);
        }

        [TestMethod]
        public void Index_ReturnsRelatedView()
        {
            // Arrange

            // Act
            var result = _cardsController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Card_WithNoExceptions_MethodCallsTokenManagerAndTrelloService()
        {
            // Arrange
            var cardId = "anicecardid";

            _mockTrelloService
                .Setup(x => x.GetCard(cardId))
                .Returns(new TrelloCard());            

            // Act
            var result = _cardsController.Card(cardId) as ViewResult;

            // Assert
            _mockTokenManager.Verify(x => x.GetUserToken(), Times.Once);
            _mockTrelloService.Verify(x => x.SetToken(_mockTokenManager.Object.GetUserToken()), Times.Once);
            _mockTrelloService.Verify(x => x.GetCard(cardId), Times.Once);
        }

        [TestMethod]
        public void Card_WithApiReturningTrelloCard_ViewResultIsBuiltWithCardModel()
        {
            // Arrange
            var cardId = "anicecardid";

            var trelloCard = new TrelloCard { Id = "aaa111", Name = "A card name", Description = "A card description" };

            _mockTrelloService
                .Setup(x => x.GetCard(cardId))
                .Returns(trelloCard);

            // Act
            var result = _cardsController.Card(cardId) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(CardModel));
            var cardModel = (CardModel)result.Model;

            Assert.AreEqual(trelloCard.Id, cardModel.Id);
            Assert.AreEqual(trelloCard.Name, cardModel.Name);
            Assert.AreEqual(trelloCard.Description, cardModel.Description);
        }

        [TestMethod]
        public void Card_WithApiThrowingException_RedirectsToHomeController()
        {
            // Arrange
            var boardId = "aniceboardid";

            _mockTrelloService
                .Setup(x => x.GetCard(boardId))
                .Throws(new TokenNotFoundException());

            // Act
            var result = _cardsController.Card(boardId) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }
    }
}