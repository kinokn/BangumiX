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

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for SubjectEpisodes.xaml
    /// </summary>
    public partial class SubjectEpisodes : UserControl
    {
        public SubjectEpisodes()
        {
            InitializeComponent();
        }

        private void SwitchButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var subject = (Model.SubjectLarge)DataContext;
            if (button.Content.ToString() == "SP") EpisodeItemsControl.ItemsSource = subject.eps_special;
            else
            {
                var item = (sender as FrameworkElement).DataContext;
                var index = SwitchButtonListView.Items.IndexOf(item);
                EpisodeItemsControl.ItemsSource = subject.eps_normal;
                EpisodeList.ScrollToVerticalOffset(index * 100  * 40);
            }
        }
    }
}
