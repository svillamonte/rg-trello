namespace RgTrello.Services.Interfaces
{
    public interface ITokenManager
    {
        void AddUserToken(string token);

        string GetUserToken();
    }
}
