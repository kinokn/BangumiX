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
using System.Diagnostics;

using bangumi_win.API;
using bangumi_win.Views;
using System.Windows.Media.Effects;

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
            CheckLogin();
            AddSubjectPage();
        }

        private async void CheckLogin()
        {
            bool logged = await HttpHelper.CheckLogin();
            if (logged)
            {
                return;
            }
            else
            {
                gridMain.Effect = new BlurEffect();
                var popupLogin = new Login();
                gridWrapper.Children.Add(popupLogin);
            }
        }

        private async void AddSubjectPage()
        {
            var subjectInfo = await HttpHelper.GetSubject(23686, 2);
            //var subjectInfo = await HttpHelper.GetSubject(218971, 2);
            var subject = new Subject(subjectInfo);
            gridMain.Children.Add(subject);
            Grid.SetColumn(subject, 3);
        }
    }
}
