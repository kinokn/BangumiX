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
        public ToolBar()
        {
            InitializeComponent();
        }

        private void SwitchToSearchClick(object sender, RoutedEventArgs e)
        {
            var search_collection = new SearchCollection();
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = search_collection;
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }

        private async void SwitchToDailyClick(object sender, RoutedEventArgs e)
        {
            var daily_list = await SwitchToDaily();
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
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }

        private async void SwitchToWatchingClick(object sender, RoutedEventArgs e)
        {
            var subject_list = await SwitchToWatching();
            var watching_collection = new WatchingCollection();
            watching_collection.Switch(ref subject_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = watching_collection;
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }

        private async void SwitchToMineClick(object sender, RoutedEventArgs e)
        {
            var collects_list = await SwitchToMine();
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
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }

        private async Task<List<Model.DailyCollection>> SwitchToDaily()
        {
            List<Model.DailyCollection> daily_list = new List<Model.DailyCollection>();
            var daily_result = await ApiHelper.GetDaily();
            if (daily_result.Status == 1)
            {
                daily_list = daily_result.DailyCollections;
            }
            return daily_list;
        }

        private async Task<List<Model.Collection>> SwitchToWatching()
        {
            List<Model.Collection> subject_list = new List<Model.Collection>();
            var watching_result = await ApiHelper.GetWatching(Settings.Default.UserID);
            if (watching_result.Status == 1)
            {
                subject_list = watching_result.Watching;
            }
            return subject_list;
        }

        private async Task<List<Model.MyCollection>> SwitchToMine()
        {
            List<Model.MyCollection> subject_list = new List<Model.MyCollection>();
            var recent_result = await ApiHelper.GetRecentCollection(Settings.Default.UserID);
            if (recent_result.Status == 1)
            {
                subject_list = recent_result.CollectWrapper[0].collects;
            }
            return subject_list;
        }

    }
}
