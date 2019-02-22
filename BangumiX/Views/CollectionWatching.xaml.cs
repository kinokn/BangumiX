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
    /// Interaction logic for Collection.xaml
    /// </summary>
    public partial class CollectionWatching : UserControl
    {
        public API.HttpHelper.WatchingResult watching_result = new API.HttpHelper.WatchingResult();
        public Subject SubjectControl;
        public CollectionWatching()
        {
            InitializeComponent();
        }
        public CollectionWatching(ref API.HttpHelper.WatchingResult ref_watching_result)
        {
            watching_result = ref_watching_result;
            InitializeComponent();
            DataContext = watching_result.Watching;
            ListViewCollections.SelectionChanged += ListViewCollectionsSelectedIndexChanged;
        }

        private async void ListViewCollectionsSelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ListViewCollections.SelectedIndex;
            if (watching_result.Watching[index].subject_detail == null)
            {
                API.HttpHelper.SubjectResult subject_result = new API.HttpHelper.SubjectResult();
                subject_result = await API.HttpHelper.GetSubject(watching_result.Watching[index].subject_id);
                if (subject_result.Status != 1) return;
                watching_result.Watching[index].subject_detail = subject_result.Subject;
            }
            if (SubjectControl == null)
            {
                SubjectControl = new Subject();
                Grid.SetColumn(SubjectControl, 1);
                GridMain.Children.Add(SubjectControl);
                SubjectControl.DataContext = watching_result.Watching[index].subject_detail;
                SubjectControl.AddSummary();
            }
            else
            {
                SubjectControl.DataContext = watching_result.Watching[index].subject_detail;
                SubjectControl.AddSummary();
            }
            return;
        }
    }
}
