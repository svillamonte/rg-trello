namespace RgTrello.Services.Trello.DTOs
{
    public interface ITrelloCard
    {
        string Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }
    }
}
