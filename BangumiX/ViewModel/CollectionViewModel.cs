using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Model;

namespace BangumiX.ViewModel
{
    public class CollectionViewModel : Common.ObservableViewModelBase
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
    }
}
