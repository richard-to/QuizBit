using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using QuizBit.Quizlet;

namespace QuizBit
{
    public partial class SignIn : PhoneApplicationPage
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App app = App.Current as App;
            AuthService authService = app.authService;
            Session session = app.session;

            string state = authService.GenerateState();
            session.SaveAuthState(state);

            WebBrowserTask task = new WebBrowserTask();
            task.Uri = authService.CreateUserAuthUri(state);
            task.Show();
        }
    }
}