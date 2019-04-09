using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Model;
using BangumiX.Common;

namespace BangumiX.ViewModel
{
    public class UserViewModel : ObservableViewModelBase
    {
        public User user = new User();

        public string Username { get { return user.username; } }
        public string Nickname { get { return user.nickname; } }
        public string AvatarSmall { get { return user.avatar["small"]; } }

        public async Task UpdateUser()
        {
            user = await ApiHelper.GetUser(Settings.UserID);
            RaisePropertyChanged(string.Empty);
        }

        public async Task<bool> CheckLogin()
        {
            return await LoginHelper.CheckLogin();
        }

        public async Task<bool> Login()
        {
            if(await LoginHelper.Login())
            {
                await UpdateUser();
                return true;
            }
            return false;
        }

        public void LogOut()
        {
            Settings.AccessToken = null;
            Settings.TokenTime = DateTimeOffset.MinValue;
            Settings.UserID = 0;
            user = new User();
            RaisePropertyChanged(string.Empty);
        }
    }
}
