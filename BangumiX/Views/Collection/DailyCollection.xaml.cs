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
    public partial class DailyCollection : UserControl
    {
        public Dictionary<uint, List<Model.Collection>> collect_list;
        public List<Model.Collection> subject_list;
        public DailyCollection()
        {
            InitializeComponent();
        }

        public void Switch(ref Dictionary<uint, List<Model.Collection>> ref_ordered_collects)
        {
            collect_list = ref_ordered_collects;
            subject_list = collect_list[7];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = SunButton.Parent;
            return;
        }

        private void MonButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[1];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void TueButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[2];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void WedButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[3];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void ThuButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[4];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void FriButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[5];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void SatButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[6];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
        private void SunButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = collect_list[7];
            SubjectListControl.SwitchList(ref subject_list);
            NavigationListView.SelectedItem = ((Button)sender).Parent;
            return;
        }
    }
}
