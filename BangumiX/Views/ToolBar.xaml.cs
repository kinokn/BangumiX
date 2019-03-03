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
using System.Windows.Controls.Primitives;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        public Model.User User;
        public ToolBar()
        {
            InitializeComponent();
            ExpandBtn.IsChecked = false;
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

        private void ToSearchClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
            ToSearch();
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }
        private void ToSearch()
        {
            var search_collection = new SearchCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = search_collection;
        }

        private async void ToWatchingClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
            await ToWatching();
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }
        private async Task ToWatching()
        {
            var watching_collection = new WatchingCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = watching_collection;
            List<Model.Collection> subject_list = new List<Model.Collection>();
            var watching_result = await ApiHelper.GetWatching(Settings.Default.UserID);
            if (watching_result.Status == 1)
            {
                subject_list = watching_result.Watching;
            }
            watching_collection.Switch(ref subject_list);
        }

        private async void ToDailyClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
            await ToDaily();
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }
        private async Task ToDaily()
        {
            var daily_collection = new DailyCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = daily_collection;
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
            List<Model.DailyCollection> daily_list = new List<Model.DailyCollection>();
            var daily_result = await ApiHelper.GetDaily();
            if (daily_result.Status == 1)
            {
                daily_list = daily_result.DailyCollections;

                foreach (var d in daily_list)
                {
                    foreach (var s in d.items)
                    {
                        ordered_daily_list[(uint)d.weekday["id"]].Add(new Model.Collection(s));
                    }
                }
            }
            daily_collection.Switch(ref ordered_daily_list);
        }

        private async void ToMineClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
            await ToMine();
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }
        private async Task ToMine()
        {
            var my_collection = new MyCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = my_collection;
            List<Model.MyCollection> collects_list = new List<Model.MyCollection>();
            Dictionary<uint, List<Model.Collection>> ordered_collects_list = new Dictionary<uint, List<Model.Collection>>()
            {
                { 1, null },
                { 2, null },
                { 3, null },
                { 4, null },
                { 5, null }
            };
            var recent_result = await ApiHelper.GetRecentCollection(Settings.Default.UserID);
            if (recent_result.Status == 1)
            {
                collects_list = recent_result.CollectWrapper[0].collects;
                foreach (var c in collects_list)
                {
                    ordered_collects_list[(uint)c.status["id"]] = c.list;
                }
            }
            my_collection.Switch(ref ordered_collects_list);
        }

        private void AvatarClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
        }

        private void DonationClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
        }

        private void SettingClick(object sender, RoutedEventArgs e)
        {
            if ((bool)ExpandBtn.IsChecked)
            {
                ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
                ExpandBtn.IsChecked = false;
            }
        }

        private void RestoreClick(object sender, RoutedEventArgs e)
        {
            ExpandBtn.RaiseEvent(new RoutedEventArgs(ToggleButton.UncheckedEvent));
            ExpandBtn.IsChecked = false;
        }
    }
}
