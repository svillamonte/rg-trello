using Ninject.Modules;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;

namespace RgTrello.Services
{
    public class ServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITokenManager>().To<TokenManager>().InSingletonScope();

            Bind<ITrelloApiClient>().To<TrelloApiClient>();
            Bind<ITrelloService>().To<TrelloService>();
        }
    }
}
