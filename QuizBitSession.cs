using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    class QuizBitSession
    {
        public const string QUIZLET_AUTH_STATE = "QuizletAuthState";
        public const string QUIZLET_ACCESS_TOKEN = "QuizletAccessToken";
        public const string QUIZLET_USER_ID = "QuizletUserID";

        private IsolatedStorageSettings _appSettings;

        public QuizBitSession(IsolatedStorageSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GetQuizletAuthState()
        {
            if (_appSettings.Contains(QUIZLET_AUTH_STATE))
            {
                return (string)_appSettings[QUIZLET_AUTH_STATE];
            }
            else
            {
                return null;
            }
        }

        public bool HasQuizletAuthState()
        {
            return _appSettings.Contains(QUIZLET_AUTH_STATE);
        }

        public void SaveQuizletAuthState(string state)
        {
            if (_appSettings.Contains(QUIZLET_AUTH_STATE))
            {
                _appSettings[QUIZLET_AUTH_STATE] = state;
            }
            else
            {
                _appSettings.Add(QUIZLET_AUTH_STATE, state);
            }
            _appSettings.Save();
        }

        public void ClearQuizletAuthState()
        {
            if (_appSettings.Contains(QUIZLET_AUTH_STATE))
            {
                _appSettings.Remove(QUIZLET_AUTH_STATE);
                _appSettings.Save();
            }
        }


        public QuizletAccessToken GetQuizletAccessToken()
        {
            if (_appSettings.Contains(QUIZLET_ACCESS_TOKEN))
            {
                QuizletAccessToken token = new QuizletAccessToken();
                token.AccessToken = (string)_appSettings[QUIZLET_ACCESS_TOKEN];
                token.UserID = (string)_appSettings[QUIZLET_USER_ID];
                return token;
            }
            else
            {
                return null;
            }
        }

        public void SaveQuizletAccessToken(QuizletAccessToken token)
        {
            if (_appSettings.Contains(QUIZLET_ACCESS_TOKEN))
            {
                _appSettings[QUIZLET_ACCESS_TOKEN] = token.AccessToken;
                _appSettings[QUIZLET_USER_ID] = token.UserID;
            }
            else
            {
                _appSettings.Add(QUIZLET_ACCESS_TOKEN, token.AccessToken);
                _appSettings.Add(QUIZLET_USER_ID, token.UserID);
            }
            _appSettings.Save();
        }

        public bool HasQuizletAccessToken()
        {
            return _appSettings.Contains(QUIZLET_ACCESS_TOKEN);
        }

        public void ClearQuizletAccessToken()
        {
            if (_appSettings.Contains(QUIZLET_ACCESS_TOKEN))
            {
                _appSettings.Remove(QUIZLET_ACCESS_TOKEN);
                _appSettings.Remove(QUIZLET_USER_ID);
                _appSettings.Save();
            }
        }
    }
}
