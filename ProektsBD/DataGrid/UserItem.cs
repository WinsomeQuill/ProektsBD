﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProektsBD.DataGrid
{
    public class UserItem
    {
        public int IdUsers { get; set; }
        public string Login { get; set; }
        public string PassWord { get; set; }
        public string NameUser { get; set; }
        public string NameRole { get; set; }
        public BitmapImage Photo { get; set; }
        public Users User { get; set; }

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
