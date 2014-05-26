using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuizBit.Quizlet
{
    class Session
    {
        public const string AUTH_STATE = "QuizletAuthState";
        public const string ACCESS_TOKEN = "QuizletAccessToken";
        public const string USER = "QuizletUser";

        private IDataProvider _appSettings;

        public Session(IDataProvider appSettings)
        {
            _appSettings = appSettings;
        }

        public string GetAuthState()
        {
            return _appSettings.Fetch(AUTH_STATE);
        }

        public void SaveAuthState(string authState)
        {
            _appSettings.Store(AUTH_STATE, authState);
        }

        public bool HasAuthState()
        {
            return _appSettings.Contains(AUTH_STATE);
        }

        public void ClearAuthState()
        {
            _appSettings.Remove(AUTH_STATE);
        }

        public AccessToken GetAccessToken()
        {
            return _appSettings.Fetch<AccessToken>(ACCESS_TOKEN);
        }

        public void SaveAccessToken(AccessToken accessToken)
        {
            _appSettings.Store<AccessToken>(ACCESS_TOKEN, accessToken);
        }

        public bool HasAccessToken()
        {
            return _appSettings.Contains(ACCESS_TOKEN);
        }

        public void ClearAccessToken()
        {
            _appSettings.Remove(ACCESS_TOKEN);
        }

        public User GetUser()
        {
            return _appSettings.Fetch<User>(USER);
        }

        public void SaveUser(User user)
        {
            _appSettings.Store<User>(USER, user);
        }

        public bool HasUser()
        {
            return _appSettings.Contains(USER);
        }

        public void ClearUser()
        {
            _appSettings.Remove(USER);
        }
    }
}
