using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProektsBD
{
    public static class UserCache
    {
        public static int Id { get; set; }
        public static string Name { get; set; }
        public static string Login { get; set; }
        public static Role Role { get; set; }
        public static BitmapImage Photo { get; set; } = null;

        public static void Reset()
        {
            Name = Login = null;
            Id = 0;
            Role = null;
            Photo = null;
        }
    }
}
