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
        public Login(API.Login login)
        {
            InitializeComponent();
            user_login = login;
            this.DataContext = user_login;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(user_login.FormHash);
            Console.WriteLine(user_login.CookieTime);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Grid parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            parent.Children.OfType<Grid>().First().Effect = null;
        }
    }
}
