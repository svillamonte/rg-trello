using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;
using RgTrello.Services.Trello.DTOs;

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
        public void SetToken()
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
    }
}
