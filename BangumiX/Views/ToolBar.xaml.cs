using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BangumiX.Properties;
using BangumiX.Common;
using System.Collections.ObjectModel;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        public Model.User User;
        public bool ToolBarExpanded;
        public ToolBar()
        {
            InitializeComponent();
            ToolBarExpanded = false;
        }

        public async void GetUser()
        {
            if (Settings.Default.AccessToken != null)
            {
                var user_result = await ApiHelper.GetUser(Settings.Default.UserID);
                User = user_result.User;
            }
            DataContext = User;
        }

        private void ExpandBtnClick(object sender, RoutedEventArgs e)
        {
            ToolBarExpanded = true;
        }

        private void ToSearchClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                ToSearchTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            ToSearch();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
        }
        private void ToSearchTextClick(object sender, RoutedEventArgs e)
        {
            ToSearch();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
            ToolBarExpanded = false;
        }
        private void ToSearch()
        {
            var search_collection = new SearchCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = search_collection;
        }

        private async void ToWatchingClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                ToWatchingTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            await ToWatching();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
        }
        private async void ToWatchingTextClick(object sender, RoutedEventArgs e)
        {
            await ToWatching();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
            ToolBarExpanded = false;
        }
        private async Task ToWatching()
        {
            List<Model.Collection> subject_list = new List<Model.Collection>();
            var watching_result = await ApiHelper.GetWatching(Settings.Default.UserID);
            if (watching_result.Status == 1)
            {
                subject_list = watching_result.Watching;
            }
            var watching_collection = new WatchingCollection();
            watching_collection.Switch(ref subject_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = watching_collection;
        }

        private async void ToDailyClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                ToDailyTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            await ToDaily();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
        }
        private async void SwitchToDailyTextClick(object sender, RoutedEventArgs e)
        {
            await ToDaily();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
            ToolBarExpanded = false;
        }
        private async Task ToDaily()
        {
            List<Model.DailyCollection> daily_list = new List<Model.DailyCollection>();
            var daily_result = await ApiHelper.GetDaily();
            if (daily_result.Status == 1)
            {
                daily_list = daily_result.DailyCollections;
            }
            Dictionary<uint, List<Model.Collection>> ordered_daily_list = new Dictionary<uint, List<Model.Collection>>()
            {
                { 1, new List<Model.Collection>() },
                { 2, new List<Model.Collection>() },
                { 3, new List<Model.Collection>() },
                { 4, new List<Model.Collection>() },
                { 5, new List<Model.Collection>() },
                { 6, new List<Model.Collection>() },
                { 7, new List<Model.Collection>() }
            };
            foreach (var d in daily_list)
            {
                foreach (var s in d.items)
                {
                    ordered_daily_list[(uint)d.weekday["id"]].Add(new Model.Collection(s));
                }
            }
            var daily_collection = new DailyCollection();
            daily_collection.Switch(ref ordered_daily_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = daily_collection;
        }

        private async void ToMineClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                ToMineTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            await ToMine();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
        }
        private async void ToMineTextClick(object sender, RoutedEventArgs e)
        {
            await ToMine();
            ToolBarListView.SelectedItem = ((Grid)((Button)sender).Parent).Parent;
            ToolBarExpanded = false;
        }
        private async Task ToMine()
        {
            List<Model.MyCollection> collects_list = new List<Model.MyCollection>();
            var recent_result = await ApiHelper.GetRecentCollection(Settings.Default.UserID);
            if (recent_result.Status == 1)
            {
                collects_list = recent_result.CollectWrapper[0].collects;
            }
            Dictionary<uint, List<Model.Collection>> ordered_collects_list = new Dictionary<uint, List<Model.Collection>>()
            {
                { 1, null },
                { 2, null },
                { 3, null },
                { 4, null },
                { 5, null }
            };
            foreach (var c in collects_list)
            {
                ordered_collects_list[(uint)c.status["id"]] = c.list;
            }
            var my_collection = new MyCollection();
            my_collection.Switch(ref ordered_collects_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = my_collection;
        }

        private void AvatarClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                AvatarTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            return;
        }
        private void AvatarTextClick(object sender, RoutedEventArgs e)
        {
            ToolBarExpanded = false;
        }

        private void DonationClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                DonationTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            return;
        }
        private void DonationTextClick(object sender, RoutedEventArgs e)
        {
            ToolBarExpanded = false;
        }

        private void SettingClick(object sender, RoutedEventArgs e)
        {
            if (ToolBarExpanded)
            {
                SettingTextBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                return;
            }
            return;
        }
        private void SettingTextClick(object sender, RoutedEventArgs e)
        {
            ToolBarExpanded = false;
        }

        private void RestoreClick(object sender, RoutedEventArgs e)
        {
            ToolBarExpanded = false;
        }
    }
}
