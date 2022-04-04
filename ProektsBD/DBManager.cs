using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProektsBD
{
    public class DBManager
    {
        public bd1Entities db = new bd1Entities();

        //переменная для хранения id пользователя, который вошел в приложение
        public static int UserId;
        //переменные для Роли, Логина, Пароля, Имени
        public static int UserRole;
        public static string Login;
        public static string Password;
        public static string Name;
        //переменная для выбора
        public static int Sel;
        //переменная для текста
        public static string Text;
        //переменная для типа
        public static int Type;
        //переменная для статуса
        public static int Status;
        //переменная для файла
        public static string file;

        // метод проверки проавильности авторизации пользователя
        public bool Auth(string login, string password)
        {
            bool Auth = false; // авторизация пока не прошла
            int id = 0; //номера пользователя пока нет
            //Получаем id по логину и паролю
            id = db.Users.AsNoTracking().Where(o => o.Login == login && o.PassWord == password).
                Select(o => o.IdUsers).FirstOrDefault();
            //Проверяем что получили id
             if(id!=0)
            {
                UserId = id; //Запоминаем id
                Auth = true; //Авторизация прошла успешно
                UserRole = db.Users.AsNoTracking().Where(o => o.IdUsers == id).Select(o => o.Role).
                    FirstOrDefault();
            }
            return Auth;
        }

        public bool Reg(string login, string password, string name, string iFile)
        {
            bool reg = false; //считаем не пройдена регистрация
            // проверяем отсутствие логина в БД 
            if (db.Users.Where(o=>o.Login==login).Select(o=>o).FirstOrDefault()==null)
            {
                //считываем фото из файла
                byte[] imagedata = null;
                FileInfo fInfo = new FileInfo(iFile);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(iFile, FileMode.Open, FileAccess.Read);
                BinaryReader bReader = new BinaryReader(fStream);
                imagedata = bReader.ReadBytes((int)numBytes);
                //получаем расширение файла изображения, не забыв удалить точку перед расширением
                string iImageExtension = (Path.GetExtension(iFile)).Replace(".", "").ToLower();

                //описываем остальные данные пользователя
                Users user = new Users
                {
                    Login = login, PassWord = password, NameUser = name, 
                    Role = 2,//это пользователь с ролью id=2, а не админ!
                    Photo = imagedata
                };
                db.Users.Add(user);//добавляем пользователя в таблицу Users
                db.SaveChanges(); //!!!! обязательно сохраняем изменения в БД
                reg = true; //рагистрация прошла успешно
            }
            return reg;
        }

        // метод создания заявки пользователя
        public void AddOrder(string text, int type)
        {
            Order order = new Order
            {
                Text = text,
                Type = type,
                Users = UserId,
                Status = 2 //заявка пользователя с ролью id=2
            };
            db.Order.Add(order); // добавляем заявку
            db.SaveChanges(); //сохраняем в БД
        }

        //метод редактирования заявки
        public void UpdateOrder (string text, int type, int status)
        {
            //описываем данные заявки
            Order order = db.Order.Where(o => o.IdOrder == Sel).First();
            order.Text = text;
            order.Type = type;
            order.Status = status;
            db.SaveChanges(); //сохраняем в БД
        }

        //метод редактирования пользователя
        public void UpdateUser(string login, string password, string name, int role, string iFile)
        { //конвертация изображения в биты
            byte[] imageData = null;
            FileInfo fInfo = new FileInfo(iFile);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(iFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            imageData = br.ReadBytes((int)numBytes);
            //описываем данные пользователя
            Users us = db.Users.Where(o => o.IdUsers == Sel).First();
            us.Login = login;
            us.PassWord = password;
            us.NameUser = name;
            us.Role = role;
            db.SaveChanges(); //сохраняем в БД
        }

        //сохранение данных заявки в переменные
        public bool dataOrder()
        {
            bool data = false;
            Order ord = db.Order.Where(o => o.IdOrder == Sel).First();
            Text = ord.Text;
            Status = ord.Status;
            Type = ord.Type;
            data = true;
            return data;
        }

        //сохранение данных пользователя в переменные
        public bool dataUser()
        {
            bool data = false;
            Users us = db.Users.Where(o => o.IdUsers == Sel).First();
            Login = us.Login;
            Password = us.PassWord;
            Name = us.NameUser;
            data = true;
            return data;
        }

        //удаление пользователя
        public void removeUser(int selected)
        {
            //находим пользователя в БД по id
            Users selectedUser = db.Users.Where(o => o.IdUsers == selected).Select(o => o).First();
            //подсчитываем заявки пользователя
            int count = db.Order.Where(o => o.Users == UserId).Select(o => o).Count();
            //в цикле удаляем заявки
            for (int i = 0; i < count; i++)
            {
                Order selOrder = db.Order.Where(o => o.Users == UserId).Select(o => o).First();
                db.Order.Remove(selOrder);
                db.SaveChanges();
            }
            //удаляем пользователя из БД
            db.Users.Remove(selectedUser);
            //сохраняем изменения в БД
            db.SaveChanges();
        }
        //удаление заявки
        public void removeOrder(int selected)
        {
            //находим заявку в БД по id
            Order selectedOrder= db.Order.Where(o => o.IdOrder == selected).Select(o => o).First();
            //удаляем заявку из БД
            db.Order.Remove(selectedOrder);
            //сохраняем изменения в БД
            db.SaveChanges();
        }
    }
}
