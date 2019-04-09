using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BangumiX.Common
{
    public static class Settings
    {
        public static ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public static string TokenType
        {
            get
            {
                return RoamingSettings.Values["TokenType"] is string ? (string)RoamingSettings.Values["TokenType"] : string.Empty;
            }
            set
            {
                RoamingSettings.Values["TokenType"] = value;
            }
        }

        public static string AccessToken
        {
            get
            {
                return RoamingSettings.Values["AccessToken"] is string ? (string)RoamingSettings.Values["AccessToken"] : string.Empty;
            }
            set
            {
                RoamingSettings.Values["AccessToken"] = value;
            }
        }

        public static string RefreshToken
        {
            get
            {
                return RoamingSettings.Values["RefreshToken"] is string ? (string)RoamingSettings.Values["RefreshToken"] : string.Empty;
            }
            set
            {
                RoamingSettings.Values["RefreshToken"] = value;
            }
        }

        public static uint Expire
        {
            get
            {
                return RoamingSettings.Values["Expire"] is uint ? (uint)RoamingSettings.Values["Expire"] : 0;
            }
            set
            {
                RoamingSettings.Values["Expire"] = value;
            }
        }

        public static DateTimeOffset TokenTime
        {
            get
            {
                return RoamingSettings.Values["TokenTime"] is DateTimeOffset ? (DateTimeOffset)RoamingSettings.Values["TokenTime"] : DateTimeOffset.MinValue;
            }
            set
            {
                RoamingSettings.Values["TokenTime"] = value;
            }
        }

        public static string ClientID
        {
            get
            {
                return "bgm7915c00e885ca2b9";
            }
        }

        public static uint UserID
        {
            get
            {
                return RoamingSettings.Values["UserID"] is uint ? (uint)RoamingSettings.Values["UserID"] : 0;
            }
            set
            {
                RoamingSettings.Values["UserID"] = value;
            }
        }
    }
}
