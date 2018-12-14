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

namespace bangumi_win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddSubjectPage();
        }

        private async void AddSubjectPage()
        {
            var subjectInfo = await HttpHelper.GetSubject(12, 2);
            var subjectPage = new Views.SubjectPage(subjectInfo);
            var subjectFrame = new Frame();
            subjectFrame.Navigate(subjectPage);
            gridMain.Children.Add(subjectFrame);
            Grid.SetColumn(subjectFrame, 2);
        }
    }
}
