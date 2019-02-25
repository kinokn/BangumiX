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
            var subject_list = await Switch(0);
            ((MainWindow)Application.Current.MainWindow).MyCollections.Switch(ref subject_list);
        }

        private async void SwitchToRecentClick(object sender, RoutedEventArgs e)
        {
            var subject_list = await Switch(1);
            ((MainWindow)Application.Current.MainWindow).MyCollections.Switch(ref subject_list);
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
    }
}
