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
        public Subject subjectControl;
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
            collectSplistView.Content = null;
            subjectControl = new Subject();
            await subjectControl.subjectVM.UpdateSubject(subjectList[index].ID);
            collectSplistView.Content = subjectControl;
            return;
        }

        private async void WishBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await collectionVM.UpdateCollection(subjectList[index].ID, "wish");
        }
        private async void WatchingBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await collectionVM.UpdateCollection(subjectList[index].ID, "do");
        }
        private async void WatchedBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await collectionVM.UpdateCollection(subjectList[index].ID, "collect");
        }
        private async void HoldBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await collectionVM.UpdateCollection(subjectList[index].ID, "on_hold");
        }
        private async void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await collectionVM.UpdateCollection(subjectList[index].ID, "dropped");
        }
    }
}
