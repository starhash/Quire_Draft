using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Quire.Resources;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace Quire
{
    public partial class SignUpPage : PhoneApplicationPage
    {
        private QuireUser u;
        private MobileServiceCollection<QuireUser, QuireUser> userCollection;
        private IMobileServiceTable<QuireUser> todoTable = App.QuireService.GetTable<QuireUser>();
        // Constructor
        public SignUpPage()
        {
            InitializeComponent();
            userPassword.Visibility = System.Windows.Visibility.Collapsed;
            userPasswordRepeat.Visibility = System.Windows.Visibility.Collapsed;
            label1.Visibility = System.Windows.Visibility.Collapsed;
            label2.Visibility = System.Windows.Visibility.Collapsed;
            signUp.Visibility = System.Windows.Visibility.Collapsed;
        }
        
        private async void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userCollection = await todoTable.Where(x => x.UserName == (userName.Text)).ToCollectionAsync();
                if (userCollection.Count != 0 || userName.Text.Equals(""))
                {
                    Color cq = new Color();
                    cq.A = 100;
                    cq.R = 255;
                    cq.G = 63;
                    cq.B = 63;
                    userName.BorderBrush = new SolidColorBrush(cq);
                    userPassword.Visibility = System.Windows.Visibility.Collapsed;
                    userPasswordRepeat.Visibility = System.Windows.Visibility.Collapsed;
                    label1.Visibility = System.Windows.Visibility.Collapsed;
                    label2.Visibility = System.Windows.Visibility.Collapsed;
                    signUp.Visibility = System.Windows.Visibility.Collapsed;
                    if (userName.Text.Equals(""))
                    {
                        mainLabel.Text = "please choose a user name";
                    }
                    else
                    {
                        mainLabel.Text = "user name already exists!";
                    }
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
                    mainLabel.Text = "user name available!";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void userName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(userName.Text.Equals("@username"))
                userName.Text = "";
        }

        private void userPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (userPassword.Password.Equals(""))
            {
                label1.Text = "password required!";
                Color cq = new Color();
                cq.A = 100;
                cq.R = 255;
                cq.G = 63;
                cq.B = 63;
                userPassword.BorderBrush = new SolidColorBrush(cq);
                label2.Visibility = System.Windows.Visibility.Collapsed;
                userPasswordRepeat.Visibility = System.Windows.Visibility.Collapsed;
                signUp.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                label1.Text = "choose a password";
                label2.Visibility = System.Windows.Visibility.Visible;
                userPasswordRepeat.Visibility = System.Windows.Visibility.Visible;
                Color cq = new Color();
                cq.A = 100;
                cq.R = 63;
                cq.G = 255;
                cq.B = 63;
                userPassword.BorderBrush = new SolidColorBrush(cq);
            }
        }

        private void userPasswordRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!userPasswordRepeat.Password.Equals(userPassword.Password))
            {
                if (userName.Text.Equals(""))
                {
                    label2.Text = "please re-enter password!";
                }
                else
                {
                    label2.Text = "passwords do not match!";
                }
                Color cq = new Color();
                cq.A = 100;
                cq.R = 255;
                cq.G = 63;
                cq.B = 63;
                userPassword.BorderBrush = new SolidColorBrush(cq);
                signUp.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                label2.Text = "passwords match!";
                Color cq = new Color();
                cq.A = 100;
                cq.R = 63;
                cq.G = 255;
                cq.B = 63;
                userPasswordRepeat.BorderBrush = new SolidColorBrush(cq);
                signUp.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                signUpProgress.IsIndeterminate = true;
                IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
                QuireUser t = new QuireUser();
                t.UserName = userName.Text;
                t.Password = userPassword.Password;
                t.Friends = "";
                t.SignUpDate = DateTime.Now.ToString();
                if (setting.Contains("userName"))
                    setting.Remove("userName");
                setting.Add("userName", t.UserName);
                await todoTable.InsertAsync(t);
                MessageBox.Show("Please Sign-In with your new credentials : " + userName.Text + " and password.");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                signUpProgress.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                signUpProgress.IsIndeterminate = false;
            }
        }
    }
}