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
        private static SubjectSummary subjectSummary;
        private static SubjectEpisodes subjectEpisodes;

        public Subject()
        {
            InitializeComponent();
        }
        public Subject(SubjectLarge s)
        {
            InitializeComponent();
            subject = s;
            DataContext = subject;
            subjectSummary = new SubjectSummary(subject);
            subjectEpisodes = new SubjectEpisodes(subject);

            gridMain.Children.Add(subjectSummary);
            Grid.SetRow(subjectSummary, 2);
        }
        private void Remove()
        {
            gridMain.Children.RemoveAt(2);
        }
        private void AddSummary()
        {
            Remove();
            gridMain.Children.Add(subjectSummary);
            Grid.SetRow(subjectSummary, 2);
        }
        private void AddEpisode()
        {
            Remove();
            gridMain.Children.Add(subjectEpisodes);
            Grid.SetRow(subjectEpisodes, 2);
        }

        private void ButtonSummary_Click(object sender, RoutedEventArgs e)
        {
            AddSummary();
        }

        private void ButtonEpisode_Click(object sender, RoutedEventArgs e)
        {
            AddEpisode();
        }

        private void ButtonCharacter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDetail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonComment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonReview_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
