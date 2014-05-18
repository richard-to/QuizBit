using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    class AssociationUriMapper : System.Windows.Navigation.UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            string tempUri = Uri.UnescapeDataString(uri.ToString());
            if (tempUri.Contains("to.richard-quizbit://authorized"))
            {
                char[] uriDelimiters = {'?', '=', '&'};
                string[] uriParameters = tempUri.Split(uriDelimiters);
                string code = "";
                string state = "";
                if (uriParameters[3] == "state")
                {
                    state = uriParameters[4];
                }

                if (uriParameters[5] == "code")
                {
                    code = uriParameters[6];
                }

                return new Uri("/MainPage.xaml?" + 
                    "code=" + code + "&" +
                    "state=" + state,
                    UriKind.Relative);
            }
            return uri;
        }
    }
}
