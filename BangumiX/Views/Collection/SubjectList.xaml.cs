using BangumiX.Common;
using BangumiX.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static BangumiX.Common.WebHelper;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for SubjectList.xaml
    /// </summary>
    public partial class SubjectList : UserControl
    {
        public List<Model.Collection> subject_list;
        public SubjectList()
        {
            InitializeComponent();
            ListViewCollections.SelectionChanged += ListViewCollectionsSelectedIndexChanged;
        }

        public void SwitchList(ref List<Model.Collection> s)
        {
            ListViewCollections.ItemsSource = null;
            ListViewCollections.SelectedIndex = -1;
            if (s != null && s.Count != 0)
            {
                subject_list = s;
                ListViewCollections.ItemsSource = subject_list;
                ListViewCollections.SelectedIndex = 0;
            }
        }

        private async void ListViewCollectionsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (subject_list == null) return;

            SubjectControl.DataContext = null;
            SubjectControl.Reset();
            SubjectControl.subject = null;
            SubjectControl.buttonSummary.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            var index = ListViewCollections.SelectedIndex;
            if (index == -1) return;
            Model.SubjectLarge subject = new Model.SubjectLarge();
            try
            {
                subject = await Retry.Do(() => ApiHelper.GetSubject(subject_list[index].subject_id), TimeSpan.FromSeconds(3));
            }
            catch (WebException web_exception)
            {
                Console.WriteLine(web_exception.Message);
                return;
            }

            SubjectControl.DataContext = subject;
            SubjectControl.subject = subject;

            Model.SubjectProgress subject_progress = new Model.SubjectProgress();
            try
            {
                subject_progress = await Retry.Do(() => ApiHelper.GetProgress(Settings.Default.UserID, subject.id), TimeSpan.FromSeconds(3));
            }
            catch (WebException web_exception)
            {
                Console.WriteLine(web_exception.Message);
            }
            catch (AuthorizationException authorization_exception)
            {
                Console.WriteLine(authorization_exception.Message);
            }
            if (subject_progress != null && subject_progress.eps != null)
            {
                foreach (var ep in subject_progress.eps)
                {
                    foreach (var ep_src in subject.eps)
                    {
                        if (ep.id == ep_src.id)
                        {
                            ep_src.ep_status = ep.status.id;
                            break;
                        }
                    }
                }
            }
            subject.eps_filter();
            return;
        }

        private async Task UpdateCollection(uint id, string status)
        {
            try
            {
                await Retry.Do(() => ApiHelper.UpdateCollection(id, status), TimeSpan.FromSeconds(3));
            }
            catch (WebException web_exception)
            {
                Console.WriteLine(web_exception.Message);
            }
            catch (AuthorizationException authorization_exception)
            {
                Console.WriteLine(authorization_exception.Message);
            }
        }

        private async void WishCollectClick(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollections.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subject_list[index].subject_id, "wish");
        }
        private async void WatchingCollectClick(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollections.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subject_list[index].subject_id, "do");
        }
        private async void WatchedCollectClick(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollections.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subject_list[index].subject_id, "collect");
        }
        private async void HoldCollectClick(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollections.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subject_list[index].subject_id, "on_hold");
        }
        private async void DropCollectClick(object sender, RoutedEventArgs e)
        {
            var index = ListViewCollections.Items.IndexOf((sender as Button).DataContext);
            await UpdateCollection(subject_list[index].subject_id, "dropped");
        }
    }
}
