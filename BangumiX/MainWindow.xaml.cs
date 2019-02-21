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

using System.Windows.Media.Effects;
using BangumiX.Properties;

namespace BangumiX
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
            if (Settings.Default.NeverAsk == true) return;
            if (Settings.Default.AccessToken != String.Empty)
            {
                var time_remain = (int) (DateTime.Now - Settings.Default.TokenTime).TotalSeconds;
                if ( time_remain < Settings.Default.Expire / 2)
                {
                    return;
                }
                else
                {
                    var refresh_token_result = await API.HttpHelper.StartRefreshToken();
                    if (refresh_token_result.Status == 1) return;
                    else Console.WriteLine("Refresh Failed");
                }
            }
            var start_login = new API.HttpHelper.StartLogin();
            await start_login.GetCaptchaSrc();
            if (start_login.captcha_src_result.Status == -1)
            {
                return;
            }
            else
            {
                //GridMain.Effect = new BlurEffect();
                var login_popup = new Views.Login(ref start_login);
                GridWrapper.Children.Add(login_popup);
                Grid.SetRowSpan(login_popup, 2);
                Grid.SetColumnSpan(login_popup, 2);
            }
        }

        private async void AddSubjectPage()
        {
            //var subject_info = await API.HttpHelper.GetSubject(23686, 2);
            var subject_info = await API.HttpHelper.GetSubject(218971, 2);
            if (subject_info.Status == 1)
            {
                var subject = new Views.Subject(subject_info.Subject);
                GridMain.Children.Add(subject);
                Grid.SetColumn(subject, 2);
            }
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
