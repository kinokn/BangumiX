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

using BangumiX.API;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class Subject : UserControl
    {
        public static SubjectSummary subject_summary;
        public static SubjectEpisodes subject_episodes;
        public static SubjectCharacters subject_characters;

        public Subject()
        {
            InitializeComponent();
            subject_summary = new SubjectSummary();
            subject_episodes = new SubjectEpisodes();
            subject_characters = new SubjectCharacters();

            GridMain.Children.Add(subject_summary);
            Grid.SetRow(subject_summary, 3);
        }
        private void Remove()
        {
            GridMain.Children.RemoveAt(3);
        }
        public void AddSummary()
        {
            Remove();
            subject_summary.DataContext = this.DataContext;
            GridMain.Children.Add(subject_summary);
            Grid.SetRow(subject_summary, 3);
        }
        public void AddEpisode()
        {
            Remove();
            subject_episodes.DataContext = this.DataContext;
            GridMain.Children.Add(subject_episodes);
            Grid.SetRow(subject_episodes, 3);
        }

        public void AddCharacter()
        {
            Remove();
            subject_characters.DataContext = this.DataContext;
            GridMain.Children.Add(subject_characters);
            Grid.SetRow(subject_characters, 3);
        }

        private void SummaryClick(object sender, RoutedEventArgs e)
        {
            AddSummary();
        }

        private void EpisodeClick(object sender, RoutedEventArgs e)
        {
            AddEpisode();
        }

        private void CharacterClick(object sender, RoutedEventArgs e)
        {
            AddCharacter();
        }

        private void DetailClick(object sender, RoutedEventArgs e)
        {

        }

        private void CommentClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReviewClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
