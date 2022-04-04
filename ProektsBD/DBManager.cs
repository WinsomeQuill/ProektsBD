using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProektsBD
{
    public static class DBManager
    {        
        public static bd1Entities db { get; } = new bd1Entities(); // создаем статическое свойство бд

        // метод проверки проавильности авторизации пользователя
        public static bool Auth(string login, string password)
        {
            Users user = db.Users.AsNoTracking()
                .Where(o => o.Login == login && o.PassWord == password)
                .Select(o => o).FirstOrDefault();

            if(user == null)
            {
                return false;
            }

            UserCache.Role = user.Role;
            UserCache.Login = user.Login;
            UserCache.Id = user.IdUsers;
            UserCache.Name = user.NameUser;

            if (user.Photo != null)
            {
                UserCache.Photo = Utils.BinaryToImage(user.Photo);
            }

            return true;
        }

        // метод проверки, существует ли пользователь с таким логином
        public static bool ExistUser(string login)
        {
            return db.Users.AsNoTracking().Where(o => o.Login == login).FirstOrDefault() != null;
        }

        // метод регистрации пользователя
        public static void Reg(string login, string password, string name, byte[] photo = null)
        {
            Users user = new Users
            {
                Login = login,
                PassWord = password,
                NameUser = name,
                Photo = photo,
                IdRole = 2
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        // метод создания заявки пользователя
        public static void AddOrder(string text, int id_type)
        {
            Order order = new Order
            {
                Text = text,
                IdType = id_type,
                IdUsers = UserCache.Id,
                IdStatus = 1,
            };
            db.Order.Add(order); // добавляем заявку
            db.SaveChanges(); //сохраняем в БД
        }

        // метод редактирования заявки
        public static void UpdateOrder (int id_order, string text, int type, int status)
        {
            Order order = db.Order.Where(o => o.IdOrder == id_order).First();
            order.Text = text;
            order.IdType = type;
            order.IdStatus = status;
            db.SaveChanges(); //сохраняем в БД
        }

        //метод редактирования пользователя
        public static void UpdateUser(int id_user, string login, string password, string name, int role, byte[] photo)
        {
            Users us = db.Users.Where(o => o.IdUsers == id_user).First();
            us.Login = login;
            us.PassWord = password;
            us.NameUser = name;
            us.IdRole = role;
            us.Photo = photo;
            db.SaveChanges(); //сохраняем в БД
        }

        //сохранение данных заявки в переменные
        [ObsoleteAttribute]
        public static bool dataOrder()
        {
            bool data = false;
            /*Order ord = db.Order.Where(o => o.IdOrder == Sel).First();
            Text = ord.Text;
            Status = ord.Status;
            Type = ord.Type;*/
            data = true;
            return data;
        }

        //сохранение данных пользователя в переменные
        [ObsoleteAttribute]
        public static bool dataUser()
        {
            bool data = false;
            /*Users us = db.Users.Where(o => o.IdUsers == Sel).First();
            Login = us.Login;
            Password = us.PassWord;
            Name = us.NameUser;*/
            data = true;
            return data;
        }

        //удаление пользователя
        public static void RemoveUser(int id_user)
        {
            //находим пользователя в БД по id
            Users user = db.Users.Where(o => o.IdUsers == id_user).Select(o => o).First();
            //подсчитываем заявки пользователя
            int count = db.Order.Where(o => o.Users == user).Select(o => o).Count();
            //в цикле удаляем заявки
            for (int i = 0; i < count; i++)
            {
                Order selOrder = db.Order.Where(o => o.Users == user).Select(o => o).First();
                db.Order.Remove(selOrder);
                db.SaveChanges();
            }
            //удаляем пользователя из БД
            db.Users.Remove(user);
            //сохраняем изменения в БД
            db.SaveChanges();
        }
        //удаление заявки
        public static void RemoveOrder(int id_order)
        {
            //находим заявку в БД по id
            Order selectedOrder= db.Order.Where(o => o.IdOrder == id_order).Select(o => o).First();
            //удаляем заявку из БД
            db.Order.Remove(selectedOrder);
            //сохраняем изменения в БД
            db.SaveChanges();
        }

        public static List<TypeOrder> GetTypeOrders()
        {
            return db.TypeOrder.ToList();
        }

        public static List<Users> GetUsers()
        {
            return db.Users.ToList();
        }

        public static List<Order> GetOrders()
        {
            return db.Order.ToList();
        }

        public static List<string> GetStatusNameOrders()
        {
            return db.Status.Select(o => o.NameStatus).ToList();
        }

        public static List<string> GetTypeNameOrders()
        {
            return db.TypeOrder.Select(o => o.NameOrder).ToList();
        }
    }
}
