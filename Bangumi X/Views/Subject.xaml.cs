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

using bangumi_win.API;

namespace bangumi_win.Views
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class Subject : UserControl
    {
        private static SubjectLarge subject = new SubjectLarge();
        private static SubjectSummary subject_summary;
        private static SubjectEpisodes subject_episodes;
        private static SubjectCharacters subject_characters;

        public Subject()
        {
            InitializeComponent();
        }
        public Subject(SubjectLarge s)
        {
            InitializeComponent();
            subject = s;
            DataContext = subject;
            subject_summary = new SubjectSummary(subject);
            subject_episodes = new SubjectEpisodes(subject);
            subject_characters = new SubjectCharacters(subject);

            GridMain.Children.Add(subject_summary);
            Grid.SetRow(subject_summary, 3);
        }
        private void Remove()
        {
            GridMain.Children.RemoveAt(3);
        }
        private void AddSummary()
        {
            Remove();
            GridMain.Children.Add(subject_summary);
            Grid.SetRow(subject_summary, 3);
        }
        private void AddEpisode()
        {
            Remove();
            GridMain.Children.Add(subject_episodes);
            Grid.SetRow(subject_episodes, 3);
        }

        private void AddCharacter()
        {
            Remove();
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
