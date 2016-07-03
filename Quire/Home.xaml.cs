using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Quire.Resources;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace Quire
{
    public partial class Home : PhoneApplicationPage
    {
        private MobileServiceCollection<QuireItem, QuireItem> quireList;
        private MobileServiceCollection<QuireItem, QuireItem> quireCollection;
        private IMobileServiceTable<QuireItem> quireTable = App.QuireService.GetTable<QuireItem>();
        private MobileServiceCollection<SharedQuireItem, SharedQuireItem> sharedQuires;
        private IMobileServiceTable<SharedQuireItem> sharedQuireTable = App.QuireService.GetTable<SharedQuireItem>();
        
        private string userLoggedIn;
        public Home()
        {
            InitializeComponent();
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            panorama.Title = setting["userName"];
            userLoggedIn = panorama.Title.ToString();
        }

        public async void Refresh()
        {
            try
            {
                MessageBox.Show(userLoggedIn);
                quireList = await quireTable.Where(x => x.UserName == (userLoggedIn)).ToCollectionAsync();
                personalList.ItemsSource = quireList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void Title_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Title Tap");
        }

        private void QuireTime_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("QuireTime Tap");
        }

        private void STitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void SQuireTime_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void addQuire_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddQuire.xaml", UriKind.Relative));
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            setting.Remove("userName");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}