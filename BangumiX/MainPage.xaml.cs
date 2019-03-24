using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
using BangumiX.View;

namespace BangumiX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ViewModel.UserViewModel UserVM;
        public MainPage()
        {
            this.InitializeComponent();
            UserVM = new ViewModel.UserViewModel();
        }

        private async void MainNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            MainNavigation.IsPaneOpen = false;
            MainNavigation.ExpandedModeThresholdWidth = int.MaxValue;
            ((NavigationViewItem)MainNavigation.SettingsItem).Content = "设置";
            if (await Common.LoginHelper.CheckLogin())
            {
                ContentFrame.Navigate(typeof(WatchingCollection));
                return;
            }
            else
            {
                ContentFrame.Navigate(typeof(DailyCollection));
                await Common.LoginHelper.Login();
            }
        }

        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem Item = args.InvokedItemContainer as NavigationViewItem;
            switch (Item.Tag)
            {
                case "NavToDaily":
                    ContentFrame.Navigate(typeof(DailyCollection));
                    break;
                case "NavToSearch":
                    ContentFrame.Navigate(typeof(SearchCollection));
                    break;
                case "NavToWatching":
                    ContentFrame.Navigate(typeof(WatchingCollection));
                    break;
                case "NavToMine":
                    ContentFrame.Navigate(typeof(MyCollection));
                    break;
            }
        }

        private async void UserInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MainNavigation.IsPaneOpen = false;
            if (await Common.LoginHelper.CheckLogin()) return;
            else await Common.LoginHelper.Login();
        }

        private void DonationBtn_Click(object sender, RoutedEventArgs e)
        {
            MainNavigation.IsPaneOpen = false;
        }
    }
}
