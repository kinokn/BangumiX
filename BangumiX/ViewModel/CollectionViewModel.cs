using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Model;
using BangumiX.Common;
using System.Net;
using static BangumiX.Common.WebHelper;

namespace BangumiX.ViewModel
{
    public class CollectionViewModel : ObservableViewModelBase
    {
        public List<SubjectViewModel> subjectList = new List<SubjectViewModel>();
        public CollectionViewModel(List<Collection> collections)
        {
            foreach (var c in collections)
            {
                subjectList.Add(new SubjectViewModel(c.subject));
            }
        }
        public CollectionViewModel(List<SubjectSmall> subjectSmalls)
        {
            foreach (var s in subjectSmalls)
            {
                subjectList.Add(new SubjectViewModel(s));
            }
        }
        public async Task UpdateCollection(uint id, string status)
        {
            try
            {
                await Retry.Do(() => ApiHelper.UpdateCollection(id, status), TimeSpan.FromSeconds(10));
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
            }
            catch (AuthorizationException authorizationException)
            {
                Console.WriteLine(authorizationException.Message);
            }
        }
    }
}
