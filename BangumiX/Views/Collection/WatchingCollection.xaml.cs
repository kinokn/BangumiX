using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BangumiX.Common;
using BangumiX.Properties;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Collection.xaml
    /// </summary>
    public partial class WatchingCollection : UserControl
    {
        public List<Model.Collection> subject_list;
        public WatchingCollection()
        {
            InitializeComponent();
        }

        public void Switch(ref List<Model.Collection> ref_ordered_collects)
        {
            SubjectListControl.SwitchList(ref ref_ordered_collects);
        }
    }
}
