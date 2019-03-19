using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Model
{
    public class SearchCollection
    {
        public uint results { get; set; }
        public List<SubjectSmall> list { get; set; }
    }
    public class DailyCollection
    {
        public Dictionary<string, dynamic> weekday { get; set; }
        public List<SubjectSmall> items { get; set; }
    }
    public class MyCollectionWrapper
    {
        public uint type { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public List<MyCollection> collects { get; set; }
    }
    public class MyCollection
    {
        public Dictionary<string, dynamic> status { get; set; }
        public string count { get; set; }
        public List<Collection> list { get; set; }
    }
    public class Collection
    {
        public string name { get; set; }
        public uint subject_id { get; set; }
        public int ep_status { get; set; }
        public ulong lasttouch { get; set; }
        public SubjectSmall subject { get; set; }
    }
}
