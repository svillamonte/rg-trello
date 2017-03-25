namespace RgTrello.Services.Trello.DTOs
{
    public class NullTrelloCard : ITrelloCard
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
