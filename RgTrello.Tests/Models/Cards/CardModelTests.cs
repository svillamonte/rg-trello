using Microsoft.VisualStudio.TestTools.UnitTesting;
using RgTrello.Models.Cards;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Tests.Models.Cards
{
    [TestClass]
    public class CardModelTests
    {
        [TestMethod]
        public void CardModelConstructor_ForTrelloCard_PopulatesPropertiesAndSetsToTrueCardWasFound()
        {
            // Arrange
            var trelloCard = new TrelloCard { Id = "aaa111", Name = "A card name", Description = "A card description" };

            // Act
            var cardModel = new CardModel(trelloCard);

            // Assert
            Assert.AreEqual(trelloCard.Id, cardModel.Id);
            Assert.AreEqual(trelloCard.Name, cardModel.Name);
            Assert.AreEqual(trelloCard.Description, cardModel.Description);

            Assert.IsTrue(cardModel.CardWasFound);
        }

        [TestMethod]
        public void CardModelConstructor_ForNullTrelloCard_SetsToFalseCardWasFound()
        {
            // Arrange
            var trelloCard = new NullTrelloCard();

            // Act
            var cardModel = new CardModel(trelloCard);

            // Assert
            Assert.IsNull(cardModel.Id);
            Assert.IsNull(cardModel.Name);
            Assert.IsNull(cardModel.Description);

            Assert.IsFalse(cardModel.CardWasFound);
        }
    }
}
