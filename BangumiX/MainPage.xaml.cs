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
        public ViewModel.UserViewModel userVM;
        public MainPage()
        {
            this.InitializeComponent();
            userVM = new ViewModel.UserViewModel();
        }

        private async void mainNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            mainNavigation.IsPaneOpen = false;
            mainNavigation.ExpandedModeThresholdWidth = int.MaxValue;
            //((NavigationViewItem)mainNavigation.SettingsItem).Content = "设置";
            if (await Common.LoginHelper.CheckLogin())
            {
                mainNavigation.SelectedItem = mainNavigation.MenuItems[1];
                ContentFrame.Navigate(typeof(WatchingCollection));
                await userVM.UpdateUser();
                return;
            }
            else
            {
                mainNavigation.SelectedItem = mainNavigation.MenuItems[2];
                ContentFrame.Navigate(typeof(DailyCollection));
                await Common.LoginHelper.Login();
            }
        }

        private void mainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
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

        private async void userInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.IsPaneOpen = false;
            if (await Common.LoginHelper.CheckLogin())
            {
                await userVM.UpdateUser();
            }
            else await Common.LoginHelper.Login();
        }

        private void donationBtn_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.IsPaneOpen = false;
        }
    }
}
