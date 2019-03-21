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

namespace BangumiX.View
{
    public sealed partial class Subject : UserControl
    {
        public ViewModel.SubjectViewModel subjectVM;
        public SubjectSummary subjectSummary;
        public SubjectEpisode subjectEpisode;
        public SubjectCharacter subjectCharacter;
        public SubjectStaff subjectStaff;
        public SubjectComment subjectComment;

        public Subject()
        {
            subjectVM = new ViewModel.SubjectViewModel();
            subjectSummary = new SubjectSummary();
            subjectEpisode = new SubjectEpisode();
            subjectCharacter = new SubjectCharacter();
            subjectStaff = new SubjectStaff();
            subjectComment = new SubjectComment();
            this.InitializeComponent();

            SubjectContentCtrl.Content = subjectSummary;
        }

        private void SummaryClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subjectSummary;
        }

        private void EpisodeClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subjectEpisode;
        }

        private void CharacterClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subjectCharacter;
        }

        private void StuffClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subjectStaff;
        }

        private void CommentClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subjectComment;
        }

        private void ProgressBtnClick(object sender, RoutedEventArgs e)
        {
            //Model.Episode item = (Model.Episode)(sender as FrameworkElement).DataContext;
            //Model.SubjectLarge subject = (Model.SubjectLarge)DataContext;
            //int offset = subject.epsOffset;
            //int index = (item.sort == "…") ? 0 : Convert.ToInt16(item.sort);
            //SubjectContentCtrl.Content = subjectEpisode;
            //subjectEpisode.EpisodeList.ScrollToVerticalOffset((index - offset) * 40);
        }

        public void Reset()
        {
            SubjectContentCtrl.Content = subjectSummary;
            //BindingOperations.SetBinding(subjectEpisode.EpisodeItemsControl, ItemsControl.ItemsSourceProperty, new Binding("eps_normal"));
            //subjectEpisode.EpisodeList.ScrollToTop();
        }
    }
}
