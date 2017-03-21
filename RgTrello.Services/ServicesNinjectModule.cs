﻿using Ninject.Modules;
using RgTrello.Services.Interfaces;

namespace RgTrello.Services
{
    public class ServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITrelloService>().To<TrelloService>();
        }
    }
}