using QuizBit.Resources;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizBit.Quizlet;

namespace QuizBit
{
    class AssociationUriMapper : System.Windows.Navigation.UriMapperBase
    {
        public const string URI_MAIN = "/MainPage.xaml";
        public const string URI_SIGNIN = "/SignIn.xaml";

        public override Uri MapUri(Uri uri)
        {
            string tempUri = Uri.UnescapeDataString(uri.ToString());
            if (tempUri.Contains(URI_SIGNIN))
            {
                return uri;
            }
            else if (tempUri.Contains(UserAppResources.QuizletInternalRedirectUri))
            {
                string[] queryString = tempUri.Split(new char[] { '?' }, 3);
                string nextUri = URI_MAIN;
                if (queryString.Length == 3)
                {
                    nextUri += "?" + queryString[2];
                }
                return new Uri(nextUri, UriKind.Relative);
            }
            else
            {
                App app = App.Current as App;
                Session session = app.session;
                if (session.HasAccessToken())
                {
                    return uri;
                }
                else
                {
                    return new Uri(URI_SIGNIN, UriKind.Relative);
                }
            }
        }
    }
}
