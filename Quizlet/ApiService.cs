using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class ApiService
    {
        public const string API_BASE_ENDPOINT = "https://api.quizlet.com/2.0/";
        public const string API_USER_ENDPOINT = API_BASE_ENDPOINT + "users/{0}";

        private AccessToken _accessToken;

        public ApiService(AccessToken accessToken)
        {
            _accessToken = accessToken;
        }

        public ApiRequest<User> fetchUser()
        {
            return new ApiRequest<User>(
                _accessToken, 
                string.Format(API_USER_ENDPOINT, _accessToken.Me));
        }
    }
}
