using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Model
{
    public class SubjectProgress
    {
        public uint subject_id { get; set; }
        public List<EpProgress> eps { get; set; }
    }
    public class EpProgress
    {
        public uint id { get; set; }
        public EpStatus status { get; set; }
    }
    public class EpStatus
    {
        public uint id { get; set; }
        public string css_name { get; set; }
        public string url_name { get; set; }
        public string cn_name { get; set; }
    }
    public class SubjectCollectStatus
    {
        public Dictionary<string, dynamic> status { get; set; }
        public uint rating { get; set; }
        public float ep_status { get; set; }
    }

    public class SubjectSmall
    {
        public uint id { get; set; }
        public string url { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public string summary { get; set; }
        public int eps { get; set; }
        public int eps_count { get; set; }
        public string air_date { get; set; }
        public int air_weekday { get; set; }
        public Rating rating { get; set; }
        public int rank { get; set; }
        public Dictionary<string, string> images { get; set; }
        public Dictionary<string, uint> collection { get; set; }
    }
    public class SubjectLarge : SubjectSmall
    {
        new public List<Episode> eps { get; set; }
        public List<Character> crt { get; set; }
        public List<Staff> staff { get; set; }
        public List<Topic> topic { get; set; }
        public List<Blog> blog { get; set; }
    }
    public class Episode
    {
        public uint id { get; set; }
        public string url { get; set; }
        public int type { get; set; }
        public string sort { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public string duration { get; set; }
        public string airdate { get; set; }
        public uint comment { get; set; }
        public string desc { get; set; }
        public string status { get; set; }
        public uint ep_status { get; set; }
    }
    public class Rating
    {
        public uint total { get; set; }
        public Dictionary<string, uint> count { get; set; }
        public float score { get; set; }
    }
    public class Character
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public string role_name { get; set; }
        public Dictionary<string, string> images { get; set; }
        public uint comment { get; set; }
        public uint collcts { get; set; }
        public Info info { get; set; }
        public List<Actor> actors { get; set; }
    }
    public class Info
    {
        public dynamic birth { get; set; }
        public dynamic height { get; set; }
        public dynamic gender { get; set; }
        public dynamic alias { get; set; }
        public dynamic source { get; set; }
        public dynamic name_cn { get; set; }
        public dynamic cv { get; set; }
    }
    //public class Info
    //{
    //    public string birth { get; set; }
    //    public string height { get; set; }
    //    public string gender { get; set; }
    //    public Dictionary<string, string> alias { get; set; }
    //    public List<string> source { get; set; }
    //    public string name_cn { get; set; }
    //    public string cv { get; set; }
    //    public Actor[] actors { get; set; }
    //}
    public class Actor
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public Dictionary<string, string> images { get; set; }
    }
    public class Staff : Character
    {
        public List<string> jobs { get; set; }
    }
    public class Topic
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public uint main_id { get; set; }
        public ulong timestamp { get; set; }
        public ulong lastpost { get; set; }
        public int replies { get; set; }
        public User user { get; set; }
    }
    public class Blog
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        public int replies { get; set; }
        public ulong timestamp { get; set; }
        public string dateline { get; set; }
        public User user { get; set; }
    }
}
