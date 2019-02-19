using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Properties;

namespace BangumiX.API
{
    public class User
    {
    }

    public class Login
    {
        public string FormHash { get; set; }
        public string CookieTime { get; set; }
        public string ChaptchaSrc { get; set; }
        public string Chaptcha { get; set; }

        private string _email;
        public string Email
        {
            get
            {
                _email = Settings.Default.Email;
                return _email;
            }
            set
            {
                _email = value;
                Settings.Default.Email = _email;
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                _password = Settings.Default.Password;
                return _password;
            }
            set
            {
                _password = value;
                if (SavePassword)
                {
                    Settings.Default.Password = _password;
                }
                else
                {
                    Settings.Default.Password = String.Empty;
                }
            }
        }

        private bool _save_password;
        public bool SavePassword
        {
            get
            {
                _save_password = Settings.Default.SavePassword;
                return _save_password;
            }
            set
            {
                _save_password = value;
                Settings.Default.SavePassword = _save_password;
            }
        }

        private bool _never_ask;
        public bool NeverAsk
        {
            get
            {
                _never_ask = Settings.Default.NeverAsk;
                return _never_ask;
            }
            set
            {
                _never_ask = value;
                Settings.Default.NeverAsk = _never_ask;
            }
        }
    }
}
