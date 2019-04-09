using BangumiX.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class WatchingCollection : Page
    {
        public WatchingCollection()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            await Refresh();
        }

        private async Task Refresh()
        {
            try
            {
                CollectionListControl.IsEnabled = false;
                collectionLoadingProgressRing.IsActive = true;
                ViewModel.CollectionViewModel collectionVM = new ViewModel.CollectionViewModel(await ApiHelper.GetWatching(Settings.UserID));
                CollectionListControl.SwitchList(ref collectionVM);
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

        private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }
    }
}
