using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BangumiX.View
{
    public sealed partial class SubjectStaff : UserControl
    {
        private ViewModel.SubjectViewModel subjectVM;
        public ViewModel.StaffViewModel staffVM;
        public SubjectStaff(ref ViewModel.SubjectViewModel s)
        {
            subjectVM = s;
            this.InitializeComponent();
        }

        private void StaffListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (staffListView.SelectedItem == null) return;
            staffVM = staffListView.SelectedItem as ViewModel.StaffViewModel;
            Bindings.Update();
        }
    }
}
