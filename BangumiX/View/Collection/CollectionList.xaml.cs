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
                subjectList = c.subjectList;
                ListViewCollection.ItemsSource = subjectList;
                ListViewCollection.SelectedIndex = 0;
            }
        }

        private async void ListViewCollectionsSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subjectList == null) return;
            SubjectControl.Reset();

            var index = ListViewCollection.SelectedIndex;
            if (index == -1) return;
            try
            {
                SubjectControl.subjectVM.UpdateSubject(await Retry.Do(() => ApiHelper.GetSubject(subjectList[index].ID), TimeSpan.FromSeconds(10)));
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                return;
            }

            Model.SubjectProgress subjectProgress = new Model.SubjectProgress();
            try
            {
                subjectProgress = await Retry.Do(() => ApiHelper.GetProgress(Settings.UserID, SubjectControl.subjectVM.ID), TimeSpan.FromSeconds(10));
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            catch (AuthorizationException authorizationException)
            {
                Console.WriteLine(authorizationException.Message);
            }
            if (subjectProgress != null && subjectProgress.eps != null)
            {
                foreach (var ep in subjectProgress.eps)
                {
                    foreach (var curEp in SubjectControl.subjectVM.Eps)
                    {
                        if (ep.id == curEp.ID)
                        {
                            curEp.EpStatus = ep.status.id;
                            break;
                        }
                    }
                }
            }
            SubjectControl.subjectVM.EpsFilter();
            return;
        }

        private async Task UpdateCollection(uint id, string status)
        {
            try
            {
                await Retry.Do(() => ApiHelper.UpdateCollection(id, status), TimeSpan.FromSeconds(3));
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            catch (AuthorizationException authorizationException)
            {
                Console.WriteLine(authorizationException.Message);
            }
        }

        private async void WishBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subjectList[index].ID, "wish");
        }
        private async void WatchingBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subjectList[index].ID, "do");
        }
        private async void WatchedBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subjectList[index].ID, "collect");
        }
        private async void HoldBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subjectList[index].ID, "on_hold");
        }
        private async void DropBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollection.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subjectList[index].ID, "dropped");
        }
    }
}
