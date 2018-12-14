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
    /// Interaction logic for SubjectPage.xaml
    /// </summary>
    public partial class SubjectPage : Page
    {
        private static SubjectLarge subject = new SubjectLarge();
        private static SubjectSummaryPage subjectSummaryPage;
        public SubjectPage(SubjectLarge s)
        {
            InitializeComponent();
            subject = s;
            DataContext = subject;
            subjectSummaryPage = new SubjectSummaryPage(subject);
            AddSummary();
        }
        private void AddSummary()
        {
            frameDetail.Navigate(subjectSummaryPage);
        }

        private void ButtonSummary_Click(object sender, RoutedEventArgs e)
        {
            AddSummary();
        }
    }
}
