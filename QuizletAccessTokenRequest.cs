using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    class QuizletAccessTokenRequest
    {
        public const string OAUTH_TOKEN_URI = "https://api.quizlet.com/oauth/token";
        public const string POST_DATA = "grant_type=authorization_code&code={0}&redirect_uri={1}";
        public const string CONTENT_TYPE = "application/x-www-form-urlencoded; charset=UTF-8";
        public const string REQUEST_METHOD = "POST";
        public const string ACCESS_TOKEN_KEY = "access_token";
        public const string USER_ID_KEY = "user_id";

        private string _clientID;
        private string _secret;
        private string _code;
        private string _redirectUri;

        public QuizletAccessTokenRequest(string clientID, string secret, string code, string redirectUri)
        {
            _clientID = clientID;
            _secret = secret;
            _code = code;
            _redirectUri = redirectUri;
        }

        public void Send(Action<QuizletAccessToken> callback)
        {
            System.Uri oauthUri = new System.Uri(OAUTH_TOKEN_URI);
            HttpWebRequest tokenRequest = (HttpWebRequest)HttpWebRequest.Create(oauthUri);

            tokenRequest.Method = REQUEST_METHOD;

            string authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(_clientID + ":" + _secret));   
            tokenRequest.Headers["Authorization"] = "Basic " + authInfo;

            tokenRequest.ContentType = CONTENT_TYPE;

            tokenRequest.BeginGetRequestStream(
                new AsyncCallback(GetRequestStreamCallback), Tuple.Create(tokenRequest, _code, _redirectUri, callback));
        }

        private void GetRequestStreamCallback(IAsyncResult result)
        {
            Tuple<HttpWebRequest, string, string, Action<QuizletAccessToken>> resultTuple =
                (Tuple<HttpWebRequest, string, string, Action<QuizletAccessToken>>)result.AsyncState;
            HttpWebRequest tokenRequest = resultTuple.Item1;
            string code = resultTuple.Item2;
            string redirectUri = resultTuple.Item3;
            Action<QuizletAccessToken> callback = resultTuple.Item4;

            Stream postStream = tokenRequest.EndGetRequestStream(result);

            string postData = string.Format(POST_DATA, code, redirectUri);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            tokenRequest.BeginGetResponse(
                new AsyncCallback(GetResponseStreamCallback), Tuple.Create(tokenRequest, callback));
        }

        private void GetResponseStreamCallback(IAsyncResult result)
        {
            Tuple<HttpWebRequest, Action<QuizletAccessToken>> resultTuple = 
                (Tuple<HttpWebRequest, Action<QuizletAccessToken>>)result.AsyncState;
            HttpWebRequest request = resultTuple.Item1;
            Action<QuizletAccessToken> callback = resultTuple.Item2;
            QuizletAccessToken oauthToken = new QuizletAccessToken();

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                oauthToken = createAccessTokenFromResponse(response);
            }
            catch (System.Net.WebException) {}

            callback(oauthToken);
        }

        public QuizletAccessToken createAccessTokenFromResponse(HttpWebResponse response)
        {
            QuizletAccessToken oauthToken = new QuizletAccessToken();
            try
            {
                StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
                string responseText = httpWebStreamReader.ReadToEnd();
                char[] uriDelimiters = { '{', ':', '}', '"', ',' };
                string[] data = responseText.Split(uriDelimiters);
                
                string lastValue = "";
                string accessToken = null;
                string userID = null;

                foreach (string value in data)
                {
                    if (lastValue.Equals(ACCESS_TOKEN_KEY) && !string.IsNullOrEmpty(value))
                    {
                        accessToken = lastValue = value;
                    }
                    else if (lastValue.Equals(USER_ID_KEY) && !string.IsNullOrEmpty(value))
                    {
                        userID = lastValue = value;
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        lastValue = value;
                    }
                }

                if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(userID))
                {
                    oauthToken.AccessToken = accessToken;
                    oauthToken.UserID = userID;
                }
            }
            catch (System.Net.WebException) {}
            return oauthToken;
        }
    }
}
