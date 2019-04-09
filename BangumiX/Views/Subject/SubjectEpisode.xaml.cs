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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BangumiX.View
{
    public sealed partial class SubjectEpisode : UserControl
    {
        private ViewModel.SubjectViewModel subjectVM;
        public SubjectEpisode(ref ViewModel.SubjectViewModel s)
        {
            subjectVM = s;
            this.InitializeComponent();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (switchListView.SelectedItem == null) return;
            //var Item =  switchListView.SelectedItem as KeyValuePair<int, string>;
            var Item = (KeyValuePair<int, string>)switchListView.SelectedItem;
            if (Item.Value == "SP") episodeListView.ItemsSource = subjectVM.EpsSpecial;
            else
            {
                episodeListView.ItemsSource = subjectVM.EpsNormal;
                episodeListView.ScrollIntoView(episodeListView.Items[Item.Key * 100], ScrollIntoViewAlignment.Leading);
            }
        }
        public void EpisodeReset()
        {
            episodeListView.ItemsSource = subjectVM.EpsNormal;
            episodeListView.SelectedIndex = -1;
            switchListView.SelectedIndex = 0;
        }
        public void ChangeSelectedEpisodeFromProgress(int index)
        {
            if (switchListView.SelectedIndex != 0)
            {
                switchListView.SelectedIndex = 0;
                episodeListView.ItemsSource = subjectVM.EpsNormal;
                episodeListView.UpdateLayout();
            }
            episodeListView.SelectedIndex = index;
            episodeListView.ScrollIntoView(episodeListView.Items[index], ScrollIntoViewAlignment.Leading);
        }
    }
}
