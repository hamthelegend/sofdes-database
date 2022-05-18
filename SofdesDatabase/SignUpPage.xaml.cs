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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SofdesDatabase
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void SignUp(object sender, RoutedEventArgs e)
        {
            var email = emailInput.Text;
            var password = passwordInput.Password;
            var name = nameInput.Text;
            var gender = genderInput.SelectedValue as string;
            if (email == string.Empty || 
                password == string.Empty ||
                name == string.Empty ||
                genderInput.SelectedIndex == -1)
            {
                await new ContentDialog
                {
                    Title = "Sign up failed",
                    Content = "None of the fields can be empty.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            if (!email.IsValidEmail())
            {
                await new ContentDialog
                {
                    Title = "Sign up failed",
                    Content = "Invalid email address.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            if (password.Length < 8)
            {
                await new ContentDialog
                {
                    Title = "Sign up failed",
                    Content = "Password must be 8-characters long.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            if (!new Regex(@"^[a-zA-Z. ]+$").IsMatch(name))
            {
                await new ContentDialog
                {
                    Title = "Sign up failed",
                    Content = "Invalid name.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            using var db = new UsersContext();
            var userWithSameEmail = db.Users.Where(user => user.Email == email).FirstOrDefault();
            if (userWithSameEmail != null)
            {
                await new ContentDialog
                {
                    Title = "Sign up failed",
                    Content = "That email was already used by someone else.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            var user = new User { 
                Email = email, 
                Password = password, 
                Name = name, 
                Gender = gender 
            };
            db.Add(user);
            db.SaveChanges();
            await new ContentDialog
            {
                Title = "Sign up successful",
                Content = "You can now sign in.",
                CloseButtonText = "Ok",
                XamlRoot = Content.XamlRoot,
            }.ShowAsync();
            Frame.GoBack();
        }
    }
}
