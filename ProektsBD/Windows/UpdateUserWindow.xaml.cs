using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProektsBD.Windows
{
    /// <summary>
    /// Логика взаимодействия для UpdateUserWindow.xaml
    /// </summary>
    public partial class UpdateUserWindow : Window
    {
        private byte[] photo { get; set; } = null;
        private Users User { get; set; }
        public UpdateUserWindow(Users user)
        {
            InitializeComponent();
            User = user;

            DBManager.db.Role.Select(o => o.NameRole).ToList().ForEach(x =>
            {
                ComboboxRole.Items.Add(x);
            });

            ComboboxRole.SelectedItem = User.Role.NameRole;

            TextboxLogin.Text = User.Login;
            Password.Password = DBManager.db.Users.Where(x => x.IdUsers == User.IdUsers).Select(x => x.PassWord).First();
            TextboxName.Text = User.NameUser;
        }

        private void ButtonAddphoto_Click(object sender, RoutedEventArgs e)
        {
            photo = Utils.ImageToBinary(Utils.GetImageWindowsDialog());
        }

        private void ButtonUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            string login = TextboxLogin.Text;
            string pass = Password.Password;
            string name = TextboxName.Text;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DBManager.UpdateUser(User.IdUsers, login, pass, name, ComboboxRole.SelectedItem as string, photo);
            Utils.UserWindow.UpdateListUsers();
            MessageBox.Show("Пользователь успешно изменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
