using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;

namespace Quire
{
    public partial class Admin : PhoneApplicationPage
    {
        private MobileServiceCollection<QuireUser, QuireUser> userList;
        private MobileServiceCollection<QuireItem, QuireItem> quireList;
        private IMobileServiceTable<QuireUser> userTable = App.QuireService.GetTable<QuireUser>();
        private IMobileServiceTable<QuireItem> quiretable = App.QuireService.GetTable<QuireItem>();

        public Admin()
        {
            InitializeComponent();
            Refresh();
        }

        public async void Refresh()
        {
            userList = await userTable.Where(x => x.UserName!=null).ToCollectionAsync();
            users.ItemsSource = userList;
            quireList = await quiretable.Where(x => x.UserName != null).ToCollectionAsync();
            quires.ItemsSource = quireList;
        }
    }
}