﻿using System.Web.Mvc;
using RgTrello.Models.Cards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class CardsController : Controller
    {
        private readonly ITrelloService _trelloService;

        public CardsController(ITrelloService trelloService)
        {
            _trelloService = trelloService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Card(string id)
        {
            try
            {
                var trelloCard = _trelloService.GetCard(id);

                return View(new CardModel((dynamic)trelloCard));
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Card(CardModel cardModel)
        {
            try
            {
                _trelloService.PostCommentToCard(cardModel.Id, cardModel.NewComment);

                return RedirectToAction("Index", "Boards");
            }
            catch
            {
                cardModel.Error = true;
                return View(cardModel);
            }
        }
    }
}