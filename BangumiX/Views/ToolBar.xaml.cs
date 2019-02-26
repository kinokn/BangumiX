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

        private async void SwitchToWatchingClick(object sender, RoutedEventArgs e)
        {
            //var subject_list = await Switch(0);
            //((MainWindow)Application.Current.MainWindow).MyCollections.Switch(ref subject_list);
            var subject_list = await SwitchToWatching();
            //var MyCollections_Navigation = new MyCollectionsNavigation();
            //MyCollections_Navigation.DataContext = subject_list;
            //((MainWindow)Application.Current.MainWindow).CurrentCollection.Switch(ref subject_list);
            //((MainWindow)Application.Current.MainWindow).MyCollections.MyCollectionsNavigationContent.Content = MyCollections_Navigation;
            var current_collection = new CurrentCollection();
            current_collection.Switch(ref subject_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = current_collection;
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
        }

        private async void SwitchToRecentClick(object sender, RoutedEventArgs e)
        {
            var collects_list = await SwitchToRecent();
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
            var my_collections = new MyCollections();
            my_collections.Switch(ref ordered_collects_list);
            ((MainWindow)Application.Current.MainWindow).CollectionContentControl.Content = my_collections;
            //((MainWindow)Application.Current.MainWindow).MyCollections.Switch(ref ordered_collects_list);
            ToolBarListView.SelectedItem = ((Button)sender).Parent;
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

        private async Task<List<Model.Collects>> SwitchToRecent()
        {
            List<Model.Collects> subject_list = new List<Model.Collects>();
            var recent_result = await ApiHelper.GetRecentCollection(Settings.Default.UserID);
            if (recent_result.Status == 1)
            {
                subject_list = recent_result.CollectWrapper[0].collects;
            }
            return subject_list;
        }

        private async Task<List<Model.Collection>> Switch(int type)
        {
            List<Model.Collection> subject_list = new List<Model.Collection>();
            if (type == 0)
            {
                var watching_result = await ApiHelper.GetWatching(Settings.Default.UserID);
                if (watching_result.Status == 1)
                {
                    subject_list = watching_result.Watching;
                }
            }
            else if (type == 1)
            {
                var recent_result = await ApiHelper.GetRecentCollection(Settings.Default.UserID);
                if (recent_result.Status == 1)
                {
                    foreach (var c in recent_result.CollectWrapper[0].collects)
                    {
                        subject_list.AddRange(c.list);
                    }
                }
            }
            return subject_list;
        }

        private void SwitchToMineClick(object sender, RoutedEventArgs e)
        {

        }

        private void SwitchToDailyClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
