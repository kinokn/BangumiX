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
    /// Interaction logic for SubjectSummary.xaml
    /// </summary>
    public partial class SubjectSummary : UserControl
    {
        public SubjectSummary()
        {
            InitializeComponent();
        }

        public SubjectSummary(SubjectLarge subject)
        {
            InitializeComponent();
            DataContext = subject;
        }
    }
}
