using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Collections;

namespace Quire
{
    public partial class AddQuire : PhoneApplicationPage
    {
        QuireItem q = new QuireItem();
        private MobileServiceCollection<QuireUser, QuireUser> userCollection;
        private IMobileServiceTable<QuireUser> todoTable = App.QuireService.GetTable<QuireUser>();
        private IMobileServiceTable<QuireItem> quireTable = App.QuireService.GetTable<QuireItem>();
        private IMobileServiceTable<SharedQuireItem> sharedQuireTable = App.QuireService.GetTable<SharedQuireItem>();
        // Constructor
        
        public AddQuire()
        {
            InitializeComponent();
            setTime.IsChecked = false;
            setTimeGrid.Visibility = System.Windows.Visibility.Collapsed;
            minutesSlider.Value = DateTime.Now.Minute;
            hourSlider.Value = DateTime.Now.Hour;
            q.Checked = false;
            q.Description = "";
            q.QuireTime = DateTime.Now.ToString();
            q.ShareTags = "";
            q.Title = "";
            q.Urgent = false;
            q.UserName = "guest";
            q.Visible = true;
            timePicker.Value = new DateTime?(DateTime.Now);
            datePicker.Value = new DateTime?(DateTime.Today);
        }

        private void hourSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                hourSel.Text = (int)hourSlider.Value + "";
                int y = DateTime.Today.Year;
                int m = DateTime.Today.Month;
                int d = DateTime.Today.Day;
                int h = (int)hourSlider.Value;
                int min = (int)minutesSlider.Value;
                DateTime dt = new DateTime(y, m, d, h, min, 0, 0);
                q.QuireTime = dt.ToString();
            }
            catch (Exception ee)
            {

            }
        }

        private void setTime_Unchecked(object sender, RoutedEventArgs e)
        {
            setTimeGrid.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void setTime_Checked(object sender, RoutedEventArgs e)
        {
            setTimeGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void minutesSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                minuteSel.Text = (int)minutesSlider.Value + "";
                int y = DateTime.Today.Year;
                int m = DateTime.Today.Month;
                int d = DateTime.Today.Day;
                int h = (int)hourSlider.Value;
                int min = (int)minutesSlider.Value;
                DateTime dt = new DateTime(y, m, d, h, min, 0, 0);
                q.QuireTime = dt.ToString();
            }
            catch(Exception ee)
            {

            }
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(title.Text.Equals(""))
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 255;
                cq.G = 63;
                cq.B = 63;
                title.BorderBrush = new SolidColorBrush(cq);
            }
            else
            {
                Color cq = new Color();
                cq.A = 100;
                cq.R = 63;
                cq.G = 255;
                cq.B = 63;
                title.BorderBrush = new SolidColorBrush(cq);
                q.Title = title.Text;
            }
        }

        private void description_TextChanged(object sender, TextChangedEventArgs e)
        {
            q.Description = description.Text;
        }

        private void urgentToggle_Checked(object sender, RoutedEventArgs e)
        {
            q.Urgent = true;
        }

        private void urgentToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            q.Urgent = false;
        }

        private void timePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DateTime? hr = timePicker.Value;
            int h = hr.Value.Hour;
            int m = hr.Value.Minute;
            int s = hr.Value.Second;
            DateTime? yy = datePicker.Value;
            int y = yy.Value.Year;
            int mo = yy.Value.Month;
            int d = yy.Value.Day;
            DateTime dd = new DateTime(y, mo, d, h, m, s, 0);
            q.QuireTime = dd.ToString();
        }

        private void datePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            DateTime? hr = timePicker.Value;
            int h = hr.Value.Hour;
            int m = hr.Value.Minute;
            int s = hr.Value.Second;
            DateTime? yy = datePicker.Value;
            int y = yy.Value.Year;
            int mo = yy.Value.Month;
            int d = yy.Value.Day;
            DateTime dd = new DateTime(y, mo, d, h, m, s, 0);
            q.QuireTime = dd.ToString();
        }

        private async void shareAuto_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                IsolatedStorageSettings s = IsolatedStorageSettings.ApplicationSettings;
                QuireUser w = (QuireUser)s["currentUser"];
                userCollection = await todoTable.Where(x => (x.UserName.Contains(shareAuto.Text)
                    && w.Friends.Contains(shareAuto.Text))).ToCollectionAsync();
                shareAuto.ItemsSource = userCollection;
            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message);
            }
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (shareAuto.Text.Length != 0)
            {
                sharedUsers.Text += " @" + shareAuto.Text;
                q.ShareTags += " @" + shareAuto.Text;
            }
        }

        private async void addQuire_Click(object sender, EventArgs e)
        {
            try
            {
                addQuireProgress.IsIndeterminate = true;
                IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
                if (q.ShareTags.IndexOf("@") == -1)
                {
                    await quireTable.InsertAsync(q);
                    MessageBox.Show("Added : " + q.Title);
                }
                else
                {
                    SharedQuireItem s = new SharedQuireItem();
                    s.Checked = q.Checked;
                    s.Description = q.Description;
                    s.QuireTime = q.QuireTime;
                    s.ShareTags = q.ShareTags;
                    s.Title = q.Title;
                    s.Urgent = q.Urgent;
                    s.UserName = q.UserName;
                    s.Visible = q.Visible;
                    s.Accepted = false;
                    s.From = q.UserName;
                    s.To = q.ShareTags;
                    await sharedQuireTable.InsertAsync(s);
                    MessageBox.Show("Added Share : " + s.Title);
                }
                NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
                addQuireProgress.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                addQuireProgress.IsIndeterminate = false;
            }
        }
    }
}