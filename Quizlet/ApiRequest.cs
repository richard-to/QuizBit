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
    class ApiRequest<T>
    {
        private AccessToken _accessToken;
        private string _endpointUrl;

        public ApiRequest(AccessToken accessToken, string endpointUrl)
        {
            _accessToken = accessToken;
            _endpointUrl = endpointUrl;
        }

        public void Send(Action<T> callback)
        {
            System.Uri uri = new System.Uri(_endpointUrl);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            request.Headers["Authorization"] = "Bearer " + _accessToken.Value;
            request.BeginGetResponse(
                new AsyncCallback(GetResponseStreamCallback), Tuple.Create(request, callback));
        }

        private void GetResponseStreamCallback(IAsyncResult result)
        {
            Tuple<HttpWebRequest, Action<T>> resultTuple =
                (Tuple<HttpWebRequest, Action<T>>)result.AsyncState;
            HttpWebRequest request = resultTuple.Item1;
            Action<T> callback = resultTuple.Item2;
            T responseData = default(T);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
                string responseText = httpWebStreamReader.ReadToEnd();
                responseData = JsonConvert.DeserializeObject<T>(responseText);
            }
            catch (System.Net.WebException e) 
            {
                string m = e.Message;
            }

            callback(responseData);
        }

    }
}
