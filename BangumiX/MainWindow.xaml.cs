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

using BangumiX.Common;

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
        }

        private async Task<bool> CheckLogin()
        {
            var check_login_result = await LoginHelper.CheckLogin();
            if (check_login_result.Status == 1) return true;
            var start_login = new LoginHelper.StartLogin();
            await start_login.GetCaptchaSrc();
            if (start_login.captcha_src_result.Status == -1)
            {
                return true;
            }
            else
            {
                var login_popup = new Views.Login(ref start_login);
                GridMain.Children.Add(login_popup);
                Grid.SetRowSpan(login_popup, 2);
                Grid.SetColumnSpan(login_popup, 2);
            }
            return false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AcrylicEffect.EnableBlur(this);
            if (await CheckLogin())
            {
                LoginHelper.APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Properties.Settings.Default.TokenType, Properties.Settings.Default.AccessToken);
                MyToolBar.ToMineBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                MyToolBar.GetUser();
            }
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }

}
