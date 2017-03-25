using RestSharp.Deserializers;

namespace RgTrello.Services.Trello.DTOs
{
    public class TrelloCard : ITrelloCard
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [DeserializeAs(Name = "desc")]
        public string Description { get; set; }
    }
}
