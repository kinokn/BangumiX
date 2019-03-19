using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BangumiX.Model;

namespace BangumiX.ViewModel
{
    public class UserViewModel : Common.ObservableViewModelBase
    {
        public User User = new User();

        public string Username { get { return User.username; } }
        public string Nickname { get { return User.nickname; } }
        public string AvatarSmall { get { return User.avatar["small"]; } }

        public void UpdateUser(User user)
        {
            User = user;
            RaisePropertyChanged(string.Empty);
        }
    }
}
