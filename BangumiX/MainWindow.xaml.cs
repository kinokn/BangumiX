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
using System.Runtime.InteropServices;
using System.Windows.Interop;

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
            //AddSubjectPage();
            AddCollectionPage(0);
        }

        private async void CheckLogin()
        {
            if (Settings.Default.NeverAsk == true) return;
            if (Settings.Default.AccessToken != String.Empty)
            {
                var time_remain = (int)(DateTime.Now - Settings.Default.TokenTime).TotalSeconds;
                if (time_remain < Settings.Default.Expire / 2)
                {
                    API.HttpHelper.APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Settings.Default.TokenType, Settings.Default.AccessToken);
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
                GridMain.Children.Add(login_popup);
                Grid.SetRowSpan(login_popup, 2);
                Grid.SetColumnSpan(login_popup, 2);
            }
        }

        private async void AddCollectionPage(int type)
        {
            if (type == 0)
            {
                var watching_result = await API.HttpHelper.GetWatching(Settings.Default.UserID);
                if (watching_result.Status == 1)
                {
                    var watching = new Views.CollectionWatching(ref watching_result);
                    GridMain.Children.Add(watching);
                    Grid.SetRow(watching, 1);
                    Grid.SetColumn(watching, 1);
                }
            }
            else if (type == 1)
            {
                var test_1 = await API.HttpHelper.GetCollection(Settings.Default.UserID);
            }
        }

        //private async void AddSubjectPage()
        //{
        //    //var subject_info = await API.HttpHelper.GetSubject(23686, 2);
        //    var subject_info = await API.HttpHelper.GetSubject(218971);
        //    if (subject_info.Status == 1)
        //    {
        //        var subject = new Views.Subject(subject_info.Subject);
        //        GridMain.Children.Add(subject);
        //        Grid.SetColumn(subject, 1);
        //    }
        //}


        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private uint _blurOpacity = 128;
        private uint _blurBackgroundColor = 0x000000; /* BGR color format */

        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            accent.GradientColor = (_blurOpacity << 24) | (_blurBackgroundColor & 0xFFFFFF);

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
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

    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }
}
