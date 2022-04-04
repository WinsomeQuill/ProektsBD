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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        DBManager manager = new DBManager();

        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void BtnRegist_Click(object sender, RoutedEventArgs e)
        {
            //Проверяем что поля ввода не пустые
            if(TextboxLoginReg.Text.Length>0 && PasswordParolReg.Password.Length>0)
            {
                if (manager.Reg(TextboxLoginReg.Text,PasswordParolReg.Password,
                    TextboxNameReg.Text,DBManager.file))
                {
                    MessageBox.Show("Регистрация прошла успешно");
                    FrameManager.MainFrame.Navigate(new AutorizationPage());
                }
            }
            else
            {
                MessageBox.Show("Заполнены не все поля");
            }
        }

        private void BtnNewPhoto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Фото";
            dlg.DefaultExt = ".png";
            Nullable<bool> result = dlg.ShowDialog();
            if (result==true)
            {
                //открываем документ
                PhotoReg.Source = new BitmapImage(new Uri(dlg.FileName));
                DBManager.file = dlg.FileName;
            }
        }
    }
}
