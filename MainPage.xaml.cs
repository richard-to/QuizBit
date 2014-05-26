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
using QuizBit.Quizlet;

namespace QuizBit
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            App app = App.Current as App;
            Session session = app.session;
            if (session.HasAccessToken())
            {
                AccessToken accessToken = session.GetAccessToken();
                SubHeader.Text = accessToken.Me;

                if (!session.HasUser())
                {
                    ApiService apiService = new ApiService(accessToken);
                    Action<User> callback = OnFetchUserSuccess;
                    ApiRequest<User> request = apiService.fetchUser();
                    request.Send(callback);
                }
                else
                {
                    User user = session.GetUser();
                    FlashcardSets.ItemsSource = user.Sets;
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("code") && NavigationContext.QueryString.ContainsKey("state"))
            {
                App app = App.Current as App;
                AuthService authService = app.authService;
                Session session = app.session;

                string state = NavigationContext.QueryString["state"];
                string code = NavigationContext.QueryString["code"];

                if (session.HasAuthState() && authService.IsValidCode(state, session.GetAuthState()))
                {
                    AccessTokenRequest tokenRequest = authService.CreateAccessTokenRequest(code, UserAppResources.QuizletRedirectUri);
                    Action<AccessToken> callback = OnAuthorizationSuccess;
                    tokenRequest.Send(callback);
                }
                session.ClearAuthState();
            }
        }

        private void OnFetchUserSuccess(User user)
        {
            if (user != null)
            {
                App app = App.Current as App;
                Session session = app.session;
                session.SaveUser(user);
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    FlashcardSets.ItemsSource = user.Sets;
                });
            }
        }

        private void OnAuthorizationSuccess(AccessToken accessToken)
        {
            if (accessToken != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    App app = App.Current as App;
                    Session session = app.session;
                    SubHeader.Text = accessToken.Me;
                    session.SaveAccessToken(accessToken);
                });
            }
        }
    }
}