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
    public sealed partial class SubjectCharacter : UserControl
    {
        private ViewModel.SubjectViewModel subjectVM;
        public ViewModel.CharacterViewModel characterVM;
        public SubjectCharacter(ref ViewModel.SubjectViewModel s)
        {
            subjectVM = s;
            this.InitializeComponent();
        }

        private void CharacterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (characterListView.SelectedItem == null) return;
            characterVM = characterListView.SelectedItem as ViewModel.CharacterViewModel;
            Bindings.Update();
        }
    }
}
