using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.API
{
    public class CollectsWrapper
    {
        public uint type { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public List<Collects> collects { get; set; }
    }
    public class Collects
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
        public SubjectLarge subject_detail { get; set; }
    }
}
