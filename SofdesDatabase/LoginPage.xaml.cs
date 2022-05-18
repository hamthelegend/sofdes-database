using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SofdesDatabase
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUpPage));
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            var email = emailInput.Text;
            var password = passwordInput.Password;
            if (email == string.Empty || password == string.Empty)
            {
                await new ContentDialog
                {
                    Title = "Login failed",
                    Content = "None of the fields can be empty.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            using var db = new UsersContext();
            var user = db.Users.Where(user => user.Email == email && user.Password == password).FirstOrDefault();
            if (user == null)
            {
                await new ContentDialog
                {
                    Title = "Login failed",
                    Content = "Email or password is incorrect.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            Frame.Navigate(typeof(DashboardPage), user);
        }
    }
}
