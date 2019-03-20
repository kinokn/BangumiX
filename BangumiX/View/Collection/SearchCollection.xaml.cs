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
    public sealed partial class SearchCollection : Page
    {
        public ViewModel.CollectionViewModel collectionVM;

        public SearchCollection()
        {
            this.InitializeComponent();
        }

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            Model.SearchCollection searchCollection = new Model.SearchCollection();
            try
            {
                searchCollection = await Retry.Do(() => ApiHelper.GetSearch(KeywordTextBox.Text), TimeSpan.FromSeconds(3));
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            catch (AuthorizationException authorizationException)
            {
                Console.WriteLine(authorizationException.Message);
            }
            catch (EmptySearchException emptySearchException)
            {
                Console.WriteLine(emptySearchException.Message);
                return;
            }
            collectionVM = new ViewModel.CollectionViewModel(searchCollection.list);
            CollectionListControl.SwitchList(ref collectionVM);
        }
    }
}
