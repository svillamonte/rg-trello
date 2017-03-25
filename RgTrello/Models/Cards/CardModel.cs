using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Models.Cards
{
    public class CardModel
    {
        public CardModel(TrelloCard trelloCard)
        {
            Id = trelloCard.Id;
            Name = trelloCard.Name;
            Description = trelloCard.Description;

            CardWasFound = true;
        }

        public CardModel(NullTrelloCard trelloCard)
        {
            CardWasFound = false;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CardWasFound { get; set; }
    }
}