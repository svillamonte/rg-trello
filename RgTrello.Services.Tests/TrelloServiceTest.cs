using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;
using RgTrello.Services.Trello.DTOs;
using System.Net;
using RgTrello.Services.Exceptions;

namespace RgTrello.Services.Tests
{
    [TestClass]
    public class TrelloServiceTest
    {
        private readonly Mock<ITrelloApiClient> _mockTrelloApiClient;
        private readonly ITrelloService _trelloService;

        public TrelloServiceTest()
        {
            _mockTrelloApiClient = new Mock<ITrelloApiClient>();

            _trelloService = new TrelloService(_mockTrelloApiClient.Object);
        }

        [TestMethod]
        public void SetToken_VerifiesClientSetTokenMethodIsCalled()
        {
            // Arrange
            var token = "anicetokentotest";

            // Act
            _trelloService.SetToken(token);

            // Assert
            _mockTrelloApiClient.Verify(x => x.SetToken(token), Times.Once);
        }

        [TestMethod]
        public void GetBoards_WithNormalBehaviour_ReturnsListOfTrelloBoards()
        {
            // Arrange
            var boardOne = new TrelloBoard { Id = "aaa111", Name = "Board one" };
            var boardTwo = new TrelloBoard { Id = "aaa222", Name = "Board two" };
            var boardThree = new TrelloBoard { Id = "aaa333", Name = "Board three" };

            var mockRestResponse = new Mock<IRestResponse<List<TrelloBoard>>>();
            mockRestResponse
                .Setup(x => x.Data)
                .Returns(new List<TrelloBoard> { boardOne, boardTwo, boardThree });

            _mockTrelloApiClient
                .Setup(x => x.Execute<List<TrelloBoard>>(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            var results = _trelloService.GetBoards();

            // Assert
            Assert.AreEqual(3, results.Count());

            Assert.AreEqual("aaa111", results.ElementAt(0).Id);
            Assert.AreEqual("Board one", results.ElementAt(0).Name);

            Assert.AreEqual("aaa222", results.ElementAt(1).Id);
            Assert.AreEqual("Board two", results.ElementAt(1).Name);

            Assert.AreEqual("aaa333", results.ElementAt(2).Id);
            Assert.AreEqual("Board three", results.ElementAt(2).Name);
        }

        [TestMethod]
        public void GetBoards_WithIssuesInCommunicationWithApi_ReturnsEmptyList()
        {
            // Arrange
            _mockTrelloApiClient
                .Setup(x => x.Execute<List<TrelloBoard>>(It.IsAny<RestRequest>()))
                .Throws(new System.Exception());

            // Act
            var results = _trelloService.GetBoards();

            // Assert
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetBoardCards_WithNormalBehaviour_ReturnsListOfTrelloCards()
        {
            // Arrange
            var cardOne = new TrelloCard { Id = "aaa111", Name = "Card one", Description = "Description one" };
            var cardTwo = new TrelloCard { Id = "aaa222", Name = "Card two", Description = "Description two" };
            var cardThree = new TrelloCard { Id = "aaa333", Name = "Card three", Description = "Description three" };

            var mockRestResponse = new Mock<IRestResponse<List<TrelloCard>>>();
            mockRestResponse
                .Setup(x => x.Data)
                .Returns(new List<TrelloCard> { cardOne, cardTwo, cardThree });

            _mockTrelloApiClient
                .Setup(x => x.Execute<List<TrelloCard>>(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            var results = _trelloService.GetBoardCards(It.IsAny<string>());

            // Assert
            Assert.AreEqual(3, results.Count());

            Assert.AreEqual("aaa111", results.ElementAt(0).Id);
            Assert.AreEqual("Card one", results.ElementAt(0).Name);
            Assert.AreEqual("Description one", results.ElementAt(0).Description);

            Assert.AreEqual("aaa222", results.ElementAt(1).Id);
            Assert.AreEqual("Card two", results.ElementAt(1).Name);
            Assert.AreEqual("Description two", results.ElementAt(1).Description);

            Assert.AreEqual("aaa333", results.ElementAt(2).Id);
            Assert.AreEqual("Card three", results.ElementAt(2).Name);
            Assert.AreEqual("Description three", results.ElementAt(2).Description);
        }

        [TestMethod]
        public void GetBoardCards_WithInvalidId_ReturnsEmptyList()
        {
            // Arrange
            var mockRestResponse = new Mock<IRestResponse<List<TrelloCard>>>();
            mockRestResponse
                .Setup(x => x.Data)
                .Returns((List<TrelloCard>)null);

            _mockTrelloApiClient
                .Setup(x => x.Execute<List<TrelloCard>>(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            var results = _trelloService.GetBoardCards(It.IsAny<string>());

            // Assert
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetBoardCards_WithIssuesInCommunicationWithApi_ReturnsEmptyList()
        {
            // Arrange
            _mockTrelloApiClient
                .Setup(x => x.Execute<List<TrelloCard>>(It.IsAny<RestRequest>()))
                .Throws(new System.Exception());

            // Act
            var results = _trelloService.GetBoardCards(It.IsAny<string>());

            // Assert
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetCard_WithNormalBehaviour_ReturnsExpectedTrelloCard()
        {
            // Arrange
            var cardOne = new TrelloCard { Id = "aaa111", Name = "Card one", Description = "Description one" };

            var mockRestResponse = new Mock<IRestResponse<TrelloCard>>();
            mockRestResponse
                .Setup(x => x.Data)
                .Returns(cardOne);

            _mockTrelloApiClient
                .Setup(x => x.Execute<TrelloCard>(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            var result = _trelloService.GetCard(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOfType(result, typeof(TrelloCard));
            Assert.AreEqual("aaa111", result.Id);
            Assert.AreEqual("Card one", result.Name);
            Assert.AreEqual("Description one", result.Description);
        }

        [TestMethod]
        public void GetCard_WithInvalidId_ReturnsNullObject()
        {
            // Arrange
            var mockRestResponse = new Mock<IRestResponse<TrelloCard>>();
            mockRestResponse
                .Setup(x => x.Data)
                .Returns((TrelloCard)null);

            _mockTrelloApiClient
                .Setup(x => x.Execute<TrelloCard>(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            var result = _trelloService.GetCard(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NullTrelloCard));
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.IsNull(result.Description);
        }

        [TestMethod]
        public void GetCard_WithIssuesInCommunicationWithApi_ReturnsNullObject()
        {
            // Arrange
            _mockTrelloApiClient
                .Setup(x => x.Execute<TrelloCard>(It.IsAny<RestRequest>()))
                .Throws(new System.Exception());

            // Act
            var result = _trelloService.GetCard(It.IsAny<string>());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NullTrelloCard));
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Name);
            Assert.IsNull(result.Description);
        }

        [TestMethod]
        public void PostCommentToCard_WithOkResponse_FollowsExpectedExecutionFlow()
        {
            // Arrange
            var mockRestResponse = new Mock<IRestResponse>();
            mockRestResponse
                .Setup(x => x.StatusCode)
                .Returns(HttpStatusCode.OK);

            _mockTrelloApiClient
                .Setup(x => x.Execute(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            _trelloService.PostCommentToCard(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            _mockTrelloApiClient.Verify(x => x.Execute(It.IsAny<RestRequest>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommentNotPostedException))]
        public void PostCommentToCard_WithUnauthorizedResponse_ThrowsException()
        {
            // Arrange
            var mockRestResponse = new Mock<IRestResponse>();
            mockRestResponse
                .Setup(x => x.StatusCode)
                .Returns(HttpStatusCode.Unauthorized);

            _mockTrelloApiClient
                .Setup(x => x.Execute(It.IsAny<RestRequest>()))
                .Returns(mockRestResponse.Object);

            // Act
            _trelloService.PostCommentToCard(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(CommentNotPostedException))]
        public void PostCommentToCard_WithIssuesInCommunicationWithApi_ThrowsException()
        {
            // Arrange
            _mockTrelloApiClient
                .Setup(x => x.Execute(It.IsAny<RestRequest>()))
                .Throws(new System.Exception());

            // Act
            _trelloService.PostCommentToCard(It.IsAny<string>(), It.IsAny<string>());
        }
    }
}
