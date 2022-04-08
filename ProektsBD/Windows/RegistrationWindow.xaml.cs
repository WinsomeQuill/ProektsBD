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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private byte[] photo { get; set; } = null;

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void BtnNewPhoto_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = Utils.GetImageWindowsDialog();
            photo = Utils.ImageToBinary(image);
            PhotoReg.Source = image;
        }

        private void BtnRegist_Click(object sender, RoutedEventArgs e)
        {
            string login = TextboxLoginReg.Text;
            string name = TextboxNameReg.Text;
            string pass = PasswordParolReg.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(pass)) // если логин или имя или пароль пустой
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(DBManager.ExistUser(login)) // если пользоатель с таким логином уже есть в БД
            {
                MessageBox.Show("Пользователь с таким логином уже зарегистрирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DBManager.Reg(login, pass, name, photo); // регистрация пользователя
            MessageBox.Show("Вы зарегистрировались!", "Успшено", MessageBoxButton.OK, MessageBoxImage.Information);
            new AuthorizationWindow().Show();
            Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new AuthorizationWindow().Show();
            Close();
        }
    }
}
