using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Quire.Resources;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Quire
{
    public partial class MainPage : PhoneApplicationPage
    {
        private QuireUser u;
        private MobileServiceCollection<QuireUser, QuireUser> userCollection;
        private IMobileServiceTable<QuireUser> todoTable = App.QuireService.GetTable<QuireUser>();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            userPassword.Visibility = System.Windows.Visibility.Collapsed;
            loginButton.Visibility = System.Windows.Visibility.Collapsed;
            label1.Visibility = System.Windows.Visibility.Collapsed;
            signInProgress.Maximum = 2;
            signInProgress.Minimum = 0;
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (userName.Text.Equals(""))
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 255;
                cq.G = 63;
                cq.B = 63;
                userName.BorderBrush = new SolidColorBrush(cq);
                userPassword.Visibility = System.Windows.Visibility.Collapsed;
                label1.Visibility = System.Windows.Visibility.Collapsed;
                signInProgress.Value = 0;
            }
            else
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 63;
                cq.G = 255;
                cq.B = 63;
                userName.BorderBrush = new SolidColorBrush(cq);
                userPassword.Visibility = System.Windows.Visibility.Visible;
                label1.Visibility = System.Windows.Visibility.Visible;
                if (userName.Text.Equals("guest"))
                {
                    userPassword.Visibility = System.Windows.Visibility.Collapsed;
                    label1.Visibility = System.Windows.Visibility.Collapsed;
                    loginButton.Visibility = System.Windows.Visibility.Visible;
                    signInProgress.Value = 2;
                }
                else
                {
                    signInProgress.Value = 1;
                }
                if (userName.Text.Equals("@username"))
                {
                    userPassword.Visibility = System.Windows.Visibility.Collapsed;
                    label1.Visibility = System.Windows.Visibility.Collapsed;
                    loginButton.Visibility = System.Windows.Visibility.Collapsed;
                    signInProgress.Value = 0;
                }
            }
        }

        private void userPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (userPassword.Password.Equals("") && !userName.Text.Equals("guest"))
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 255;
                cq.G = 63;
                cq.B = 63;
                userPassword.BorderBrush = new SolidColorBrush(cq);
                loginButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 63;
                cq.G = 255;
                cq.B = 63;
                userPassword.BorderBrush = new SolidColorBrush(cq);
                loginButton.Visibility = System.Windows.Visibility.Visible;
                signInProgress.Value = 2;
            }
        }

        private void userName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(userName.Text.Equals("@username"))
                userName.Text = "";
        }


        private void signUp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUpPage.xaml", UriKind.Relative));
        }

        private async void loginButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                signInProgress.IsIndeterminate = true;
                userCollection = await todoTable.Where(x => x.UserName == (userName.Text)).ToCollectionAsync();
                if (userCollection.Count != 0)
                {
                    IsolatedStorageSettings s = IsolatedStorageSettings.ApplicationSettings;
                    u = userCollection.ElementAt<QuireUser>(0);
                    if (userPassword.Password.Equals(u.Password))
                    {
                        if (s.Contains("userName"))
                        {
                            s.Remove("userName");
                        }
                        s.Add("userName", userName.Text);
                        if(userName.Text.Equals("quireadmin"))
                        {
                            NavigationService.Navigate(new Uri("/Admin.xaml", UriKind.Relative));
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
                        }
                    }
                    else
                        throw new Exception("Please check the username/password");
                    s.Add("currentUser", u);
                }
                else
                {
                    throw new Exception("Please check the username/password");
                }
                signInProgress.IsIndeterminate = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                userPassword.Password = "";
                userName.Focus();
                signInProgress.IsIndeterminate = false;
            }
        }

        private void userName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (userName.Text.Equals(""))
            {
                userName.Text = "@username";
                userPassword.Visibility = System.Windows.Visibility.Collapsed;
                loginButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            
        }

        
    }
}