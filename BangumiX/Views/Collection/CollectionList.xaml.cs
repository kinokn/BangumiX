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
using BangumiX.Common;
using System.Net;
using static BangumiX.Common.WebHelper;
using System.Threading.Tasks;

namespace BangumiX.View
{
    public sealed partial class CollectionList : UserControl
    {
        public List<ViewModel.SubjectViewModel> subjectList;
        public ViewModel.CollectionViewModel collectionVM;
        public CollectionList()
        {
            this.InitializeComponent();
            ListViewCollection.SelectionChanged += ListViewCollectionsSelectedIndexChanged;
        }

        public void SwitchList(ref ViewModel.CollectionViewModel c)
        {
            ListViewCollection.ItemsSource = null;
            ListViewCollection.SelectedIndex = -1;
            if (c != null && c.subjectList.Count != 0)
            {
                collectionVM = c;
                subjectList = collectionVM.subjectList;
                ListViewCollection.ItemsSource = subjectList;
                ListViewCollection.SelectedIndex = 0;
            }
        }

        private async void ListViewCollectionsSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ListViewCollection.SelectedIndex;
            if (subjectList == null || index == -1) return;
            try
            {
                subjectControl.IsEnabled = false;
                subjectControl.subjectProgressRing.IsActive = true;
                await subjectControl.subjectVM.UpdateSubject(subjectList[index].ID);
            }
            finally
            {
                subjectControl.subjectProgressRing.IsActive = false;
                subjectControl.IsEnabled = true;
                subjectControl.pivotSubject.SelectedIndex = 0;
                subjectControl.Visibility = Visibility.Visible;
            }
        }

        private async void WishBtn_Click(object sender, RoutedEventArgs e)
        {
            var subjectVM = (sender as MenuFlyoutItem).DataContext as ViewModel.SubjectViewModel;
            await collectionVM.UpdateCollection(subjectVM.ID, "wish");
        }
        private async void WatchingBtn_Click(object sender, RoutedEventArgs e)
        {
            var subjectVM = (sender as MenuFlyoutItem).DataContext as ViewModel.SubjectViewModel;
            await collectionVM.UpdateCollection(subjectVM.ID, "do");
        }
        private async void WatchedBtn_Click(object sender, RoutedEventArgs e)
        {
            var subjectVM = (sender as MenuFlyoutItem).DataContext as ViewModel.SubjectViewModel;
            await collectionVM.UpdateCollection(subjectVM.ID, "collect");
        }
        private async void HoldBtn_Click(object sender, RoutedEventArgs e)
        {
            var subjectVM = (sender as MenuFlyoutItem).DataContext as ViewModel.SubjectViewModel;
            await collectionVM.UpdateCollection(subjectVM.ID, "on_hold");
        }
        private async void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            var subjectVM = (sender as MenuFlyoutItem).DataContext as ViewModel.SubjectViewModel;
            await collectionVM.UpdateCollection(subjectVM.ID, "dropped");
        }
    }
}
