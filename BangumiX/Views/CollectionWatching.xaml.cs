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

using BangumiX.Common;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Collection.xaml
    /// </summary>
    public partial class CollectionWatching : UserControl
    {
        public List<Model.Collection> subject_list;
        public Subject SubjectControl;
        public CollectionWatching()
        {
            InitializeComponent();
            ListViewCollections.SelectionChanged += ListViewCollectionsSelectedIndexChanged;
        }

        public void Switch(ref List<Model.Collection> collections)
        {
            subject_list = collections;
            DataContext = subject_list;
        }

        private async void ListViewCollectionsSelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ListViewCollections.SelectedIndex;
            if (index == -1) return;
            if (subject_list[index].subject_detail == null)
            {
                HttpHelper.SubjectResult subject_result = new HttpHelper.SubjectResult();
                subject_result = await HttpHelper.GetSubject(subject_list[index].subject_id);
                if (subject_result.Status != 1) return;
                subject_list[index].subject_detail = subject_result.Subject;
            }
            if (SubjectControl == null)
            {
                SubjectControl = new Subject();
                Grid.SetColumn(SubjectControl, 1);
                GridMain.Children.Add(SubjectControl);
                SubjectControl.DataContext = subject_list[index].subject_detail;
                SubjectControl.buttonSummary.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                SubjectControl.DataContext = subject_list[index].subject_detail;
                SubjectControl.buttonSummary.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            return;
        }
    }
}
