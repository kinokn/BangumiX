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

namespace bangumi_win.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public string username { get; set; }
        public string password { get; set; }

        public Login()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(username);
            Debug.WriteLine(password);
            username = "test1";
            password = "test2";
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            Grid parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            parent.Children.OfType<Grid>().First().Effect = null;
        }
    }
}
