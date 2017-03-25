using Microsoft.VisualStudio.TestTools.UnitTesting;
using RgTrello.Controllers;
using System.Web.Mvc;

namespace RgTrello.Tests.Controllers
{
    [TestClass]
    public class CardsControllerTests
    {
        private readonly CardsController _cardsController;

        public CardsControllerTests()
        {
            _cardsController = new CardsController();
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
    }
}
