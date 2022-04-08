using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProektsBD.DataGrid
{
    public class UserItem
    {
        public int IdUsers { get; set; } // id пользоателя
        public string Login { get; set; } // логин пользоателя
        public string PassWord { get; set; } // пароль пользователя
        public string NameUser { get; set; } // имя пользователя
        public string NameRole { get; set; } // названи роли пользователя
        public BitmapImage Photo { get; set; } // фотография пользователя
        public Users User { get; set; } // пользователь

        public UserItem(Users user)
        {
            IdUsers = user.IdUsers;
            Login = user.Login;
            PassWord = user.PassWord;
            NameUser = user.NameUser;
            NameRole = user.Role.NameRole;
            Photo = Utils.BinaryToImage(user.Photo);
            User = user;
        }
    }
}
