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
    public sealed partial class DashboardPage : Page
    {
        User user;
        public DashboardPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = (User) e.Parameter;
        }

        private async void DeleteAccount(object sender, RoutedEventArgs e)
        {
            var response = await new ContentDialog
            {
                Title = "Delete account",
                Content = "Are you sure you want to delete this account?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                XamlRoot = Content.XamlRoot,
            }.ShowAsync();
            
            if (response == ContentDialogResult.Primary)
            {
                using var db = new UsersContext();
                db.Remove(user);
                await new ContentDialog
                {
                    Title = "Account deleted",
                    Content = "You can no longer sign in with this account.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                db.SaveChanges();
                Frame.GoBack();
            }
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            var password = passwordInput.Password;
            var name = nameInput.Text;
            var gender = genderInput.SelectedValue as string;
            if (password == string.Empty ||
                name == string.Empty ||
                genderInput.SelectedIndex == -1)
            {
                await new ContentDialog
                {
                    Title = "Update failed",
                    Content = "None of the fields can be empty.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            if (password.Length < 8)
            {
                await new ContentDialog
                {
                    Title = "Update failed",
                    Content = "Password must be 8-characters long.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            if (!name.IsValidName())
            {
                await new ContentDialog
                {
                    Title = "Update failed",
                    Content = "Invalid name.",
                    CloseButtonText = "Ok",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return;
            }
            using var db = new UsersContext();
            user.Password = password;
            user.Name = name;
            user.Gender = gender;
            db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            await new ContentDialog
            {
                Title = "Update successful",
                Content = "Changes to your credentials were saved successfully.",
                CloseButtonText = "Ok",
                XamlRoot = Content.XamlRoot,
            }.ShowAsync();
            return;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
