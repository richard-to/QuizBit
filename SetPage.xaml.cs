using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using QuizBit.Quizlet;
using System.Windows.Media.Imaging;

namespace QuizBit
{
    public partial class TermListPage : PhoneApplicationPage
    {
        public TermListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("setID"))
            {
                string setID = NavigationContext.QueryString["setID"];

                App app = App.Current as App;
                Session session = app.session;
                if (session.HasAccessToken())
                {
                    AccessToken accessToken = session.GetAccessToken();
                    ApiService apiService = new ApiService(accessToken);
                    Action<Set> callback = OnFetchTermsSuccess;
                    ApiRequest<Set> request = apiService.fetchSetTerms(setID);
                    request.Send(callback);
                }
            }
        }

        private void OnFetchTermsSuccess(Set set)
        {
            if (set != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    SubHeader.Text = set.Title;
                    LLS_Terms.ItemsSource = set.Terms;
                });
            }
        }

    }
}