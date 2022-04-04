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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProektsBD
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public DBManager manager = new DBManager();

        public AutorizationPage()
        {
            InitializeComponent();
        }

        // нажатие на кнопку Зарегистрироваться
        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new RegistrationPage());
        }

        // Нажатие ни кнопку Войти
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {   // проверка что пароль и логин не пустые
            if(TextboxLogin.Text.Length>0 && PasswordParol.Password.Length>0)
            {
                // проверка правильности введенных логина и пароля
                if(manager.Auth(TextboxLogin.Text, PasswordParol.Password))
                {
                    FrameManager.MainFrame.Navigate(new UserPage());
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль");
                }
            }
            else 
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}


