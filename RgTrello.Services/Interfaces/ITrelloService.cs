﻿using System.Collections.Generic;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Services.Interfaces
{
    public interface ITrelloService
    {
        IEnumerable<TrelloBoard> GetBoards();

        IEnumerable<TrelloCard> GetBoardCards(string boardId);

        ITrelloCard GetCard(string cardId);

        void PostCommentToCard(string cardId, string commentText);
    }
}
