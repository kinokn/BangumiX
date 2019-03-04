using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
using static BangumiX.Common.WebHelper;

namespace BangumiX.Views
{
    /// <summary>
    /// Interaction logic for Collection.xaml
    /// </summary>
    public partial class SearchCollection : UserControl
    {
        public List<Model.Collection> subject_list;
        public SearchCollection()
        {
            InitializeComponent();
        }

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            subject_list = new List<Model.Collection>();
            Model.SearchCollection search_collection = new Model.SearchCollection();
            try
            {
                search_collection = await Retry.Do(() => ApiHelper.GetSearch(KeywordTextBox.Text), TimeSpan.FromSeconds(3));
            }
            catch (WebException web_exception)
            {
                Console.WriteLine(web_exception.Message);
            }
            catch (AuthorizationException authorization_exception)
            {
                Console.WriteLine(authorization_exception.Message);
            }
            catch (EmptySearchException empty_search_exception)
            {
                Console.WriteLine(empty_search_exception.Message);
                return;
            }
            foreach (var s in search_collection.list)
            {
                subject_list.Add(new Model.Collection(s));
            }
            SubjectListControl.SwitchList(ref subject_list);
        }
    }
}
