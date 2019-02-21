using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BangumiX.Properties;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public API.HttpHelper.StartLogin start_login;
        public Login()
        {
        }
        public Login(ref API.HttpHelper.StartLogin ref_start_login)
        {
            start_login = ref_start_login;
            InitializeComponent();
            this.DataContext = start_login.login;
            CaptchaImg.Source = start_login.captcha_src_result.CaptchaSrc;
        }

        private void RemoveSelf()
        {
            start_login.Shutdown();
            Grid parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            parent.Children.OfType<Grid>().First().Effect = null;
        }

        private async void CaptchaClick(object sender, RoutedEventArgs e)
        {
            start_login.captcha_src_result = new API.HttpHelper.CaptchaSrcResult();
            await start_login.GetCaptchaSrc();
            if (start_login.captcha_src_result.Status == 1)
            {
                CaptchaImg.Source = start_login.captcha_src_result.CaptchaSrc;
            }
            else Console.WriteLine("Captcha Failed");
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            await start_login.Start();
            if (start_login.login_result.Status == 1) RemoveSelf();
            else Console.WriteLine("Login Failed");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            RemoveSelf();
        }
    }
}
