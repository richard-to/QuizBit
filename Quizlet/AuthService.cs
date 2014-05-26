using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class AuthService
    {
        public const string USER_AUTH_URI = "https://quizlet.com/authorize?response_type=code&client_id={0}&scope={1}&state={2}";
        public const string STATE_PARAM = "state";
        public const string CODE_PARAM = "code";

        private string _clientID;
        private string _secret;
        private string _scope;

        public AuthService(string clientID, string secret, string scope)
        {
            _clientID = clientID;
            _secret = secret;
            _scope = scope;
        }

        public string GenerateState()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData).Replace("=", "");
        }

        public Uri CreateUserAuthUri(string state)
        {
            return new Uri(string.Format(USER_AUTH_URI, _clientID, _scope, state));
        }

        public bool IsValidCode(string givenState, string expectedState)
        {
            return givenState.Equals(expectedState);
        }

        public AccessTokenRequest CreateAccessTokenRequest(string code, string redirectUri)
        {
            return new AccessTokenRequest(_clientID, _secret, code, redirectUri);          
        }
    }
}
