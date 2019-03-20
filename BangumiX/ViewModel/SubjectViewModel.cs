using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace BangumiX.ViewModel
{
    public class SubjectViewModel : Common.ObservableViewModelBase
    {
        public SubjectLarge subject;
        public List<EpisodeViewModel> _Eps { get; set; }
        public List<CharacterViewModel> _Crts { get; set; }
        public SubjectViewModel()
        {
            subject = new SubjectLarge();
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
        }
        public SubjectViewModel(SubjectSmall s)
        {
            subject = new SubjectLarge
            {
                id = s.id,
                name = s.name,
                name_cn = s.name_cn,
                images = s.images
            };
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
        }
        public SubjectViewModel(SubjectLarge s)
        {
            subject = new SubjectLarge();
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
            if (s.eps != null)
            {
                foreach (var e in s.eps)
                {
                    _Eps.Add(new EpisodeViewModel(e));
                }
            }
            if (s.crt != null)
            {
                foreach (var c in s.crt)
                {
                    _Crts.Add(new CharacterViewModel(c));
                }
            }
            subject = s;
        }

        public uint ID => subject.id;
        public string Type
        {
            get
            {
                switch (subject.type)
                {
                    case 1:
                        return "Book";
                    case 2:
                        return "Anime";
                    case 3:
                        return "Music";
                    case 4:
                        return "Game";
                    case 6:
                        return "Real";
                }
                return string.Empty;
            }
        }
        public string Name => subject.name == string.Empty ? null : subject.name;
        public string NameCN => subject.name_cn == string.Empty ? null : subject.name_cn;
        public string Summary => subject.summary;
        public int EpsCount => subject.eps_count;
        public string AirDate => subject.air_date;
        public string AirWeekDay
        {
            get
            {
                switch (subject.air_weekday)
                {
                    case 1:
                        return "周一";
                    case 2:
                        return "周二";
                    case 3:
                        return "周三";
                    case 4:
                        return "周四";
                    case 5:
                        return "周五";
                    case 6:
                        return "周六";
                    case 7:
                        return "周日";
                }
                return string.Empty;
            }
        }
        public uint TotalRating => subject.rating.total;
        public Dictionary<string, float> RatingCount
        {
            get
            {
                var _count = new Dictionary<string, float>();
                foreach (var c in subject.rating.count.Keys)
                {
                    {
                        _count.Add(c, (float)subject.rating.count[c] / (float)TotalRating * 100);
                    }
                }
                return _count;
            }
        }
        public float Score => subject.rating.score;
        public int Rank => subject.rank;
        public BitmapImage ImageSmall
        {
            get
            {
                if (subject.images == null) return null;
                subject.images.TryGetValue("small", out string img);
                return new BitmapImage(new Uri(img));
            }
        }
        public BitmapImage ImageGrid
        {
            get
            {
                if (subject.images == null) return null;
                subject.images.TryGetValue("grid", out string img);
                return new BitmapImage(new Uri(img));
            }
        }
        public BitmapImage ImageLarge
        {
            get
            {
                if (subject.images == null) return null;
                subject.images.TryGetValue("large", out string img);
                return new BitmapImage(new Uri(img));
            }
        }
        public uint ToTalCollection => (uint)subject.collection.Sum(x => x.Value);
        public List<EpisodeViewModel> Eps => _Eps;
        public List<CharacterViewModel> Crt => _Crts;

        public int EpsOffset { get; set; }
        public List<Episode> EpsNormal { get; set; }
        public List<Episode> EpsSpecial { get; set; }
        public List<Episode> EpsSub { get; set; }
        public Visibility ButtonVisibility { get; set; }
        public Dictionary<int, string> ButtonCount { get; set; }
        public void EpsFilter()
        {
            if (subject.eps == null) return;
            EpsNormal = new List<Episode>();
            EpsSpecial = new List<Episode>();
            EpsSub = new List<Episode>();
            bool offsetFlag = true;
            foreach (var e in subject.eps)
            {
                if (e.type == 0)
                {
                    if (offsetFlag)
                    {
                        EpsOffset = Convert.ToInt16(e.sort);
                        EpsOffset = EpsOffset < 0 ? 0 : EpsOffset;
                        offsetFlag = false;
                    }
                    if (EpsSub.Count < 27)
                    {
                        EpsSub.Add(e);
                    }
                    EpsNormal.Add(e);
                }
                else
                {
                    EpsSpecial.Add(e);
                }
            }
            if (EpsSub.Count > 26)
            {
                EpsSub.Add(new Episode() { sort = "…", status = "Air" });
            }
            RaisePropertyChanged("EpsSub");
            RaisePropertyChanged("EpsNormal");
            RaisePropertyChanged("EpsSpecial");

            ButtonCount = new Dictionary<int, string>();
            int num = EpsNormal.Count;
            int buttonNum = 0;
            while (num > 0)
            {
                int begin = buttonNum * 100 + EpsOffset;
                int end = num < 100 ? buttonNum * 100 + num + EpsOffset - 1 : (buttonNum + 1) * 100 + EpsOffset - 1;
                ButtonCount.Add(buttonNum, string.Format("{0} - {1}", begin, end));
                buttonNum += 1;
                num -= 100;
            }
            if (EpsSpecial.Count > 0)
            {
                ButtonCount.Add(buttonNum, "SP");
            }
            if (ButtonCount.Count > 1) ButtonVisibility = Visibility.Visible;
            else ButtonVisibility = Visibility.Collapsed;
            RaisePropertyChanged("ButtonVisibility");
            RaisePropertyChanged("ButtonCount");
        }
        public void UpdateSubject(SubjectLarge s)
        {
            subject = s;
            EpsFilter();
            RaisePropertyChanged(string.Empty);
        }
    }

    public class EpisodeViewModel : Common.ObservableViewModelBase
    {
        public Episode episode;
        public EpisodeViewModel(Episode e)
        {
            episode = e;
        }
        public uint ID => episode.id;
        public string Sort => episode.sort;
        public string Name => episode.name == string.Empty ? null : episode.name;

        public string NameCN => episode.name_cn == string.Empty ? null : episode.name_cn;
        public string FullName
        {
            get
            {
                string s = episode.sort + ". ";
                if (episode.name_cn != string.Empty) s = s + episode.name_cn + " \\ ";
                s += episode.name;
                return s;
            }
        }
        public Visibility FullNameVisibility => FullName.Length > 43 ? Visibility.Visible : Visibility.Collapsed;
        public uint _EpStatus { get; set; }
        public uint EpStatus
        {
            get
            {
                return _EpStatus;
            }
            set
            {
                _EpStatus = value;
                RaisePropertyChanged("EpStatus");
            }
        }
    }

    public class CharacterViewModel : Common.ObservableViewModelBase
    {
        public Character character;
        public CharacterViewModel() { }
        public CharacterViewModel(Character c)
        {
            character = c;
        }
        public uint ID => character.id;
        public string Name => character.name == string.Empty ? null : character.name;
        public string NameCN => character.name_cn == string.Empty ? null : character.name_cn;
        public BitmapImage ImageGrid
        {
            get
            {
                if (character.images == null) return null;
                character.images.TryGetValue("grid", out string img);
                return new BitmapImage(new Uri(img ?? "https://bangumi.tv/img/info_only.png"));
            }
        }
        public dynamic Birth => character.info.birth;
        public dynamic Height => character.info.height;
        public dynamic Gender => character.info.gender;
        public dynamic Alias => character.info.alias;
    }

    public class StaffViewModel : CharacterViewModel
    {
        public List<string> JobList;
        public StaffViewModel(Staff s)
        {
            base.character = s;
            JobList = s.jobs;
        }
        public string Jobs => string.Join(" ", JobList.ToArray());
    }
}
