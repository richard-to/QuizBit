using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class AccessTokenRequest
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

        public AccessTokenRequest(string clientID, string secret, string code, string redirectUri)
        {
            _clientID = clientID;
            _secret = secret;
            _code = code;
            _redirectUri = redirectUri;
        }

        public void Send(Action<AccessToken> callback)
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
            Tuple<HttpWebRequest, string, string, Action<AccessToken>> resultTuple =
                (Tuple<HttpWebRequest, string, string, Action<AccessToken>>)result.AsyncState;
            HttpWebRequest tokenRequest = resultTuple.Item1;
            string code = resultTuple.Item2;
            string redirectUri = resultTuple.Item3;
            Action<AccessToken> callback = resultTuple.Item4;

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
            Tuple<HttpWebRequest, Action<AccessToken>> resultTuple = 
                (Tuple<HttpWebRequest, Action<AccessToken>>)result.AsyncState;
            HttpWebRequest request = resultTuple.Item1;
            Action<AccessToken> callback = resultTuple.Item2;
            AccessToken oauthToken = new AccessToken();

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                oauthToken = createAccessTokenFromResponse(response);
            }
            catch (System.Net.WebException) {}

            callback(oauthToken);
        }

        public AccessToken createAccessTokenFromResponse(HttpWebResponse response)
        {
            AccessToken accessToken = null;
            try
            {
                StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
                string responseText = httpWebStreamReader.ReadToEnd();
                accessToken = JsonConvert.DeserializeObject<AccessToken>(responseText);
            }
            catch (System.Net.WebException) {}
            return accessToken;
        }
    }
}
