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
    }
}
