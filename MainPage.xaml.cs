using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using QuizBit.Resources;
using System.Text;
using System.IO;
using Microsoft.Phone.Tasks;

namespace QuizBit
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            App app = App.Current as App;
            QuizBitSession session = app.session;
            if (session.HasQuizletAccessToken() && session.GetQuizletAccessToken().IsValid())
            {
                SubHeader.Text = session.GetQuizletAccessToken().UserID;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("code") && NavigationContext.QueryString.ContainsKey("state"))
            {
                App app = App.Current as App;
                QuizletAuthService authService = app.authService;
                QuizBitSession session = app.session;

                string state = NavigationContext.QueryString["state"];
                string code = NavigationContext.QueryString["code"];

                if (session.HasQuizletAuthState() && authService.IsValidCode(state, session.GetQuizletAuthState()))
                {
                    QuizletAccessTokenRequest tokenRequest = authService.CreateAccessTokenRequest(code, UserAppResources.QuizletRedirectUri);
                    Action<QuizletAccessToken> callback = OnAuthorizationSuccess;
                    tokenRequest.Send(callback);
                }
                session.ClearQuizletAuthState();
            }
        }

        private void OnAuthorizationSuccess(QuizletAccessToken token)
        {
            if (token.IsValid())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App app = App.Current as App;
                    QuizBitSession session = app.session;
                    SubHeader.Text = token.UserID;
                    session.SaveQuizletAccessToken(token);
                });
            }
        }
    }
}