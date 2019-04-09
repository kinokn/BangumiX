using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BangumiX.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using BangumiX.Common;
using static BangumiX.Common.WebHelper;

namespace BangumiX.ViewModel
{
    public class SubjectViewModel : ObservableViewModelBase
    {
        private SubjectLarge subject;
        private List<EpisodeViewModel> _Eps { get; set; }
        private List<CharacterViewModel> _Crts { get; set; }
        private List<StaffViewModel> _Staffs { get; set; }
        private SubjectCollectViewModel _subjectCollectStatus { get; set; }
        public SubjectViewModel()
        {
            subject = new SubjectLarge();
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
            _Staffs = new List<StaffViewModel>();
            _subjectCollectStatus = new SubjectCollectViewModel();
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
            _Staffs = new List<StaffViewModel>();
            _subjectCollectStatus = new SubjectCollectViewModel();
        }
        public SubjectViewModel(SubjectLarge s)
        {
            subject = new SubjectLarge();
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
            _Staffs = new List<StaffViewModel>();
            _subjectCollectStatus = new SubjectCollectViewModel();
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
            if (s.staff != null)
            {
                foreach (var staff in s.staff)
                {
                    _Staffs.Add(new StaffViewModel(staff));
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
        public string AirWeekday
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
        public uint TotalRating
        {
            get
            {
                if (subject.rating == null) return 0;
                return subject.rating.total;
            }
        }
        public string TotalRatingString
        {
            get
            {
                return string.Format("{0}人评分", TotalRating);
            }
        }
        public Dictionary<string, float> RatingCount
        {
            get
            {
                if (subject.rating == null) return null;
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
        public string Score
        {
            get
            {
                if (subject.rating == null) return "暂无评分";
                return subject.rating.score.ToString("N1");
            }
        }
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
        public uint ToTalCollection
        {
            get
            {
                if (subject.collection == null || subject.collection.Count == 0) return 0;
                return (uint)subject.collection.Sum(x => x.Value);
            }
        }
        public List<EpisodeViewModel> Eps => _Eps;
        public List<CharacterViewModel> Crt => _Crts;
        public List<StaffViewModel> Staff => _Staffs;
        public SubjectCollectViewModel SubjectCollectStatus => _subjectCollectStatus;

        public int EpsOffset { get; set; }
        public List<EpisodeViewModel> EpsNormal { get; set; }
        public List<EpisodeViewModel> EpsSpecial { get; set; }
        public List<EpisodeViewModel> EpsSub { get; set; }
        public Visibility ButtonVisibility { get; set; }
        public Dictionary<int, string> ButtonCount { get; set; }
        public void EpsFilter()
        {
            if (_Eps == null) return;
            EpsNormal = new List<EpisodeViewModel>();
            EpsSpecial = new List<EpisodeViewModel>();
            EpsSub = new List<EpisodeViewModel>();
            bool offsetFlag = true;
            foreach (var ep in _Eps)
            {
                if (ep.Type == 0)
                {
                    if (offsetFlag)
                    {
                        EpsOffset = Convert.ToInt16(ep.Sort);
                        EpsOffset = EpsOffset < 0 ? 0 : EpsOffset;
                        offsetFlag = false;
                    }
                    if (EpsSub.Count < 27)
                    {
                        EpsSub.Add(ep);
                    }
                    EpsNormal.Add(ep);
                }
                else
                {
                    EpsSpecial.Add(ep);
                }
            }
            if (EpsSub.Count > 26)
            {
                EpsSub.Add(new EpisodeViewModel
                (
                    new Episode()
                    {
                        sort = "…",
                        status = "Air"
                    }
                ));
            }
            //RaisePropertyChanged("EpsSub");
            //RaisePropertyChanged("EpsNormal");
            //RaisePropertyChanged("EpsSpecial");

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
            //RaisePropertyChanged("ButtonVisibility");
            //RaisePropertyChanged("ButtonCount");
        }

        public async Task UpdateMultipleProgress()
        {
            try
            {
                if (_subjectCollectStatus.ChangedEpStatus == _subjectCollectStatus.EpStatus.ToString()) return;
                await ApiHelper.UpdateMultipleProgress(subject.id, _subjectCollectStatus.ChangedEpStatus);
                await UpdateSubject(ID);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                return;
            }
        }

        public async Task UpdateSubject(uint sID)
        {
            SubjectLarge subjectLarge = new SubjectLarge();
            try
            {
                subjectLarge = await ApiHelper.GetSubject(sID);
            }
            catch (WebException webException)
            {
                Console.WriteLine(webException.Message);
                return;
            }

            subject = subjectLarge;
            _Eps = new List<EpisodeViewModel>();
            _Crts = new List<CharacterViewModel>();
            _Staffs = new List<StaffViewModel>();
            if (subjectLarge.eps != null)
            {
                foreach (var e in subjectLarge.eps)
                {
                    _Eps.Add(new EpisodeViewModel(e));
                }
            }
            if (subjectLarge.crt != null)
            {
                foreach (var c in subjectLarge.crt)
                {
                    _Crts.Add(new CharacterViewModel(c));
                }
            }
            if (subjectLarge.staff != null)
            {
                foreach (var staff in subjectLarge.staff)
                {
                    _Staffs.Add(new StaffViewModel(staff));
                }
            }

            if (Settings.AccessToken != string.Empty)
            {

                SubjectCollectStatus subjectCollectStatus = new SubjectCollectStatus();
                try
                {
                    subjectCollectStatus = await ApiHelper.GetCollection(sID);
                }
                catch (WebException webException)
                {
                    Console.WriteLine(webException.Message);
                }
                catch (AuthorizationException authorizationException)
                {
                    Console.WriteLine(authorizationException.Message);
                }

                SubjectProgress subjectProgress = new SubjectProgress();
                try
                {
                    subjectProgress = await ApiHelper.GetProgress(Settings.UserID, sID);
                }
                catch (WebException webException)
                {
                    Console.WriteLine(webException.Message);
                }
                catch (AuthorizationException authorizationException)
                {
                    Console.WriteLine(authorizationException.Message);
                }

                _subjectCollectStatus = new SubjectCollectViewModel(subjectCollectStatus);
                if (subjectProgress != null && subjectProgress.eps != null)
                {
                    foreach (var ep in subjectProgress.eps)
                    {
                        foreach (var curEp in _Eps)
                        {
                            if (ep.id == curEp.ID)
                            {
                                curEp.EpStatus = ep.status.id;
                                break;
                            }
                        }
                    }
                }
            }
            EpsFilter();
            RaisePropertyChanged(string.Empty);
        }
    }

    public class EpisodeViewModel : ObservableViewModelBase
    {
        public Episode episode;
        public EpisodeViewModel() { }
        public EpisodeViewModel(Episode e)
        {
            episode = e;
        }
        public uint ID => episode.id;
        public int Type => episode.type;
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
        public Visibility FullNameVisibility => FullName.Length > 50 ? Visibility.Visible : Visibility.Collapsed;
        public string Status
        {
            get { return episode.status; }
            set { episode.status = value; }
        }
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
        public SolidColorBrush EpStatusColor
        {
            get
            {
                if (EpStatus == 2) return GetSolidColorBrush("#FF004EB4");
                switch (Status)
                {
                    case "Air":
                        return GetSolidColorBrush("#FF4093FF");
                    case "Today":
                        return GetSolidColorBrush("#FF4093FF");
                }
                return GetSolidColorBrush("#FF3F3F3F");
            }
        }
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }

    public class CharacterViewModel : ObservableViewModelBase
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
                if (character.images == null) return new BitmapImage(new Uri("https://bangumi.tv/img/info_only.png"));
                character.images.TryGetValue("grid", out string img);
                return new BitmapImage(new Uri(img ?? "https://bangumi.tv/img/info_only.png"));
            }
        }
        public BitmapImage ImageLarge
        {
            get
            {
                if (character.images == null) return new BitmapImage(new Uri("https://bangumi.tv/img/info_only.png"));
                character.images.TryGetValue("large", out string img);
                return new BitmapImage(new Uri(img ?? "https://bangumi.tv/img/info_only.png"));
            }
        }
        public dynamic Birth => character.info?.birth;
        public dynamic Height => character.info?.height;
        public dynamic Gender => character.info?.gender;
        //public dynamic Alias => character.info.alias;
        public string NameCNString => NameCN != null ? "中文名：" + NameCN : null;
        public string NameString => Name != null ? "日文名：" + Name : null;
        //public string NameEnString => character.info != null && character.info.alias != null ? "英文名：" + Alias["en"] : null;
        public string GenderString => Gender != null ? "性别：" + Gender : null;
        public string BirthString => Birth != null ? "生日：" + Birth : null;
        public string HeightString => Height != null ? "身高：" + Height : null;
        public string CVs
        {
            get
            {
                if (character.actors == null) return string.Empty;
                string cv = "声优：";
                foreach (var actor in character.actors)
                {
                    cv = cv + " " + actor.name;
                }
                return cv;
            }
        }
    }

    public class StaffViewModel : CharacterViewModel
    {
        public List<string> JobList;
        public StaffViewModel()
        {
            base.character = new Character();
        }
        public StaffViewModel(Staff s)
        {
            base.character = s;
            JobList = s.jobs;
        }
        public string Jobs => "职业：" + string.Join(" ", JobList.ToArray());
    }

    public class SubjectCollectViewModel : ObservableViewModelBase
    {
        private SubjectCollectStatus subjectCollectStatus;
        public SubjectCollectViewModel()
        {
            subjectCollectStatus = new SubjectCollectStatus();
        }
        public SubjectCollectViewModel(SubjectCollectStatus s)
        {
            subjectCollectStatus = s;
            ChangedEpStatus = subjectCollectStatus.ep_status.ToString();
        }
        public Visibility StatusVisibility => subjectCollectStatus.status == null ? Visibility.Collapsed : Visibility.Visible;
        public string Status => subjectCollectStatus.status == null ? string.Empty : string.Format("我{0}这部动画", subjectCollectStatus.status["name"]);
        public uint Rating => subjectCollectStatus.rating;
        public float EpStatus => subjectCollectStatus.ep_status;
        private string _changedEpStatus { get; set; }
        public string ChangedEpStatus
        {
            get
            {
                return _changedEpStatus; 
            }
            set
            {
                _changedEpStatus = value;
            }
        }
    }
}
