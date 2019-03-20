using BangumiX.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using static BangumiX.Common.WebHelper;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BangumiX.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyCollection : Page
    {
        //public List<Model.MyCollection> myCollection;
        public Dictionary<uint, ViewModel.CollectionViewModel> myCollection;
        public ViewModel.CollectionViewModel curCollection;
        public MyCollection()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            try
            {
                await Retry.Do(async () =>
                {
                    List<Model.MyCollection> myCollectionResult = await ApiHelper.GetMyCollection(Settings.UserID);
                    myCollection = new Dictionary<uint, ViewModel.CollectionViewModel>()
                    {
                        { 1, null },
                        { 2, null },
                        { 3, null },
                        { 4, null },
                        { 5, null }
                    };
                    foreach (var c in myCollectionResult)
                    {
                        myCollection[(uint)c.status["id"]] = new ViewModel.CollectionViewModel(c.list);
                    }
                    curCollection = myCollection[3];
                    CollectionListControl.SwitchList(ref curCollection);
                }, TimeSpan.FromSeconds(10));
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (curCollection == null) return;
            var Item = myCollectionListView.SelectedItem as ListViewItem;
            switch (Item.Tag)
            {
                case "Wish":
                    curCollection = myCollection[1];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Watching":
                    curCollection = myCollection[3];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Watched":
                    curCollection = myCollection[2];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Hold":
                    curCollection = myCollection[4];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
                case "Drop":
                    curCollection = myCollection[5];
                    CollectionListControl.SwitchList(ref curCollection);
                    break;
            }
        }
    }
}
