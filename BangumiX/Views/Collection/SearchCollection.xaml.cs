using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using BangumiX.Properties;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Collection.xaml
    /// </summary>
    public partial class SearchCollection : UserControl
    {
        public List<Model.Collection> subject_list;
        public Subject SubjectControl;
        public SearchCollection()
        {
            InitializeComponent();
            ListViewCollections.SelectionChanged += ListViewCollectionsSelectedIndexChanged;
        }

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            ListViewCollections.ItemsSource = null;
            var search_result = await ApiHelper.GetSearch(KeywordTextBox.Text);
            if (search_result.Status == 1)
            {
                foreach (var s in search_result.SearchCollections.list)
                {
                    subject_list.Add(new Model.Collection(s));
                }
            }
            ListViewCollections.ItemsSource = subject_list;
        }

        public async void ListViewCollectionsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (subject_list == null) return;
            var index = ListViewCollections.SelectedIndex;
            if (index == -1) return;
            var subject = subject_list[index];
            ApiHelper.SubjectResult subject_result = new ApiHelper.SubjectResult();
            subject_result = await ApiHelper.GetSubject(subject_list[index].subject_id);
            if (subject_result.Status != 1) return;

            subject_list[index].subject_detail = subject_result.Subject;
            if (SubjectControl == null)
            {
                SubjectControl = new Subject();
                Grid.SetColumn(SubjectControl, 1);
                GridMain.Children.Add(SubjectControl);
                //SubjectControl.DataContext = subject_list[index].subject_detail;
                SubjectControl.DataContext = subject;
                SubjectControl.buttonSummary.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                //SubjectControl.DataContext = subject_list[index].subject_detail;
                SubjectControl.DataContext = subject;
                SubjectControl.buttonSummary.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

            ApiHelper.ProgressResult progress_result = new ApiHelper.ProgressResult();
            progress_result = await ApiHelper.GetProgress(Settings.Default.UserID, subject_result.Subject.id);
            if (progress_result.Status == 1)
            {
                if (progress_result.SubjectProgress != null)
                {
                    if (progress_result.SubjectProgress.eps != null)
                    {
                        foreach (var ep_src in subject_result.Subject.eps_2)
                        {
                            foreach (var ep in progress_result.SubjectProgress.eps)
                            {
                                if (ep.id == ep_src.ID) ep_src.EpStatus = ep.status.id;
                            }
                        }
                    }
                }
            }
            return;
        }

    }
}
