using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using Newtonsoft.Json;

namespace RgTrello.Auth
{
    public class TrelloClient : OAuthClient
    {
        private const string RequestTokenUrl = "https://trello.com/1/OAuthGetRequestToken";
        private const string AccessTokenUrl = "https://trello.com/1/OAuthGetAccessToken";
        private const string AuthorizeTokenUrl = "https://trello.com/1/OAuthAuthorizeToken";
        private const string ProfileUrl = "https://api.trello.com/1/members/me";

        public TrelloClient(string consumerKey, string consumerSecret, string appName)
            : base("Trello", CreateServiceProviderDescription(appName), consumerKey, consumerSecret)
        {
        }

        protected override AuthenticationResult VerifyAuthenticationCore(AuthorizedTokenResponse response)
        {
            var profileEndpoint = new MessageReceivingEndpoint(ProfileUrl, HttpDeliveryMethods.GetRequest);
            var request = WebWorker.PrepareAuthorizedRequest(profileEndpoint, response.AccessToken);

            try
            {
                using (var profileResponse = request.GetResponse())
                using (var profileResponseStream = profileResponse.GetResponseStream())
                using (var profileStreamReader = new StreamReader(profileResponseStream))
                {
                    var profileStreamContents = profileStreamReader.ReadToEnd();
                    var profile = JsonConvert.DeserializeObject<Dictionary<string, object>>(profileStreamContents);

                    return new AuthenticationResult(
                            isSuccessful: true,
                            provider: ProviderName,
                            providerUserId: (string)profile["id"],
                            userName: (string)profile["username"],
                            extraData: GetExtraData(profile));
                }
            }
            catch (Exception exception)
            {
                return new AuthenticationResult(exception);
            }
        }

        private static Dictionary<string, string> GetExtraData(Dictionary<string, object> profile)
        {
            return profile.ToDictionary(kp => kp.Key, kp => kp.Value?.ToString());
        }

        private static ServiceProviderDescription CreateServiceProviderDescription(string appName)
        {
            var deliveryMethods = HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest;

            return new ServiceProviderDescription
            {
                RequestTokenEndpoint = new MessageReceivingEndpoint(RequestTokenUrl, deliveryMethods),
                AccessTokenEndpoint = new MessageReceivingEndpoint(AccessTokenUrl, deliveryMethods),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint($"{AuthorizeTokenUrl}?name={HttpUtility.UrlEncode(appName)}&scope=read,write", deliveryMethods),
                TamperProtectionElements = new[] { new HmacSha1SigningBindingElement() }
            };
        }
    }
}