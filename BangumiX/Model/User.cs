using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BangumiX.Common;

namespace BangumiX.Model
{
    public class User
    {
        public User()
        {
            username = null;
            nickname = "未登录";
            avatar = new Dictionary<string, string>() { { "small", "http://lain.bgm.tv/pic/user/s/icon.jpg" } };
        }
        public string username { get; set; }
        public string nickname { get; set; }
        public Dictionary<string, string> avatar { get; set; }
    }

    public class Token
    {
        public uint user_id
        {
            get
            {
                return Settings.UserID;
            }
            set
            {
                Settings.UserID = value;
            }
        }
        public string access_token
        {
            get
            {
                return Settings.AccessToken;
            }
            set
            {
                Settings.AccessToken = value;
            }
        }
        public uint expires_in
        {
            get
            {
                return Settings.Expire;
            }
            set
            {
                Settings.Expire = value;
            }
        }
        public string refresh_token
        {
            get
            {
                return Settings.RefreshToken;
            }
            set
            {
                Settings.RefreshToken = value;
            }
        }
        public DateTimeOffset token_time
        {
            get
            {
                return Settings.TokenTime;
            }
            set
            {
                Settings.TokenTime = value;
            }
        }
        public string token_type
        {
            get
            {
                return Settings.TokenType;
            }
            set
            {
                Settings.TokenType = value;
            }
        }
    }
}
