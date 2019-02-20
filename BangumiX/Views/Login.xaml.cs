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
using System.Windows.Media.Imaging;
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
        private API.Login user_login = new API.Login();
        public Login()
        {
            InitializeComponent();
        }
        public Login(string captcha_src)
        {
            InitializeComponent();
            byte[] binaryData = Convert.FromBase64String(captcha_src);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.EndInit();
            user_login.CaptchaSrc = bi;
            this.DataContext = user_login;
        }

        private void RemoveSelf()
        {
            Grid parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            parent.Children.OfType<Grid>().First().Effect = null;
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            var login_result = await API.HttpHelper.StartLogin.Start(user_login);
            if (login_result.Status == 1) RemoveSelf();
            else Console.WriteLine("Login Failed");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            RemoveSelf();
        }
    }
}
