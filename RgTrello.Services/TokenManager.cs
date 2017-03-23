using System.Collections.Generic;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Exceptions;

namespace RgTrello.Services
{
    public class TokenManager : ITokenManager
    {
        private const string Key = "accesstoken";
        private IDictionary<string, string> _tokenDictionary;

        public TokenManager()
        {
            _tokenDictionary = new Dictionary<string, string>();
        }

        public void AddUserToken(string token)
        {            
            if (_tokenDictionary.ContainsKey(Key))
            {
                return;
            }

            _tokenDictionary.Add(Key, token);
        }

        public string GetUserToken()
        {
            try
            {
                return _tokenDictionary[Key];
            }
            catch (KeyNotFoundException)
            {
                throw new TokenNotFoundException();
            }
        }
    }
}
