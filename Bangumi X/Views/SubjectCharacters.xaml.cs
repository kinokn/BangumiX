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
    /// Interaction logic for SubjectCharacters.xaml
    /// </summary>
    public partial class SubjectCharacters : UserControl
    {
        public SubjectCharacters()
        {
            InitializeComponent();
        }

        public SubjectCharacters(SubjectLarge subject)
        {
            InitializeComponent();
            DataContext = subject;
        }

    }
}
