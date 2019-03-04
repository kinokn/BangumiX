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
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class Subject : UserControl
    {
        public Model.SubjectLarge subject;
        public SubjectSummary subject_summary;
        public SubjectEpisodes subject_episodes;
        public SubjectCharacters subject_characters;
        public SubjectStaff subject_staff;
        public SubjectComment subject_comment;

        public Subject()
        {
            subject = new Model.SubjectLarge();
            subject_summary = new SubjectSummary();
            subject_episodes = new SubjectEpisodes();
            subject_characters = new SubjectCharacters();
            subject_staff = new SubjectStaff();
            subject_comment = new SubjectComment();
            InitializeComponent();

            SubjectContentCtrl.Content = subject_summary;
        }

        private void SummaryClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subject_summary;
        }

        private void EpisodeClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subject_episodes;
        }

        private void CharacterClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subject_characters;
        }

        private void StuffClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subject_staff;
        }

        private void CommentClick(object sender, RoutedEventArgs e)
        {
            SubjectContentCtrl.Content = subject_comment;
        }

        private void ProgressBtnClick(object sender, RoutedEventArgs e)
        {
            Model.Episode item = (Model.Episode)(sender as FrameworkElement).DataContext;
            Model.SubjectLarge subject = (Model.SubjectLarge)DataContext;
            int offset = subject.eps_offset;
            int index = (item.sort == "…") ? 0 : Convert.ToInt16(item.sort);
            SubjectContentCtrl.Content = subject_episodes;
            subject_episodes.EpisodeList.ScrollToVerticalOffset((index - offset) * 40);
        }

        public void Reset()
        {
            BindingOperations.SetBinding(subject_episodes.EpisodeItemsControl, ItemsControl.ItemsSourceProperty, new Binding("eps_normal"));
            subject_episodes.EpisodeList.ScrollToTop();
        }
    }
}
