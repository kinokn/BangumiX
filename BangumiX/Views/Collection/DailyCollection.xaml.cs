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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
using BangumiX.Common;
using System.Net;
using static BangumiX.Common.WebHelper;

namespace BangumiX.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DailyCollection : Page
    {
        public Dictionary<uint, ViewModel.CollectionViewModel> dailyCollection;
        public ViewModel.CollectionViewModel curCollection;
        public DailyCollection()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                CollectionListControl.IsEnabled = false;
                collectionLoadingProgressRing.IsActive = true;
                List<Model.DailyCollection> myCollectionResult = await ApiHelper.GetDaily();
                dailyCollection = new Dictionary<uint, ViewModel.CollectionViewModel>()
                {
                    { 1, null },
                    { 2, null },
                    { 3, null },
                    { 4, null },
                    { 5, null },
                    { 6, null },
                    { 7, null }
                };
                foreach (var c in myCollectionResult)
                {
                    dailyCollection[(uint)c.weekday["id"]] = new ViewModel.CollectionViewModel(c.items);
                }
                curCollection = dailyCollection[7];
                CollectionListControl.SwitchList(ref curCollection);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            catch (AuthorizationException authorizationException)
            {
                Console.WriteLine(authorizationException.Message);
            }
            finally
            {
                collectionLoadingProgressRing.IsActive = false;
                CollectionListControl.IsEnabled = true;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (curCollection == null) return;
            var Item = myCollectionListView.SelectedItem as ListViewItem;
            switch (Item.Tag)
            {
                case "Mon":
                    curCollection = dailyCollection[1];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Tue":
                    curCollection = dailyCollection[3];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Wed":
                    curCollection = dailyCollection[2];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Thu":
                    curCollection = dailyCollection[4];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Fri":
                    curCollection = dailyCollection[5];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Sat":
                    curCollection = dailyCollection[6];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Sun":
                    curCollection = dailyCollection[7];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
            }
        }
    }
}
