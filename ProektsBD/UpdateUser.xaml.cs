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
    /// Логика взаимодействия для UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Page
    {
        DBManager manager = new DBManager();
        public UpdateUser()
        {
            InitializeComponent();
            ComboboxRole.SelectedIndex = DBManager.UserRole-1;
            ComboboxRole.ItemsSource = manager.db.Role.Select(o => o.NameRole).ToList();
            TextboxLogin.Text = DBManager.Login;
            Password.Password = DBManager.Password;
            TextboxName.Text = DBManager.Name;
        }

        private void ButtonAddphoto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName="Фото"; //имя файла по умолчанию
            dlg.DefaultExt = ".png"; //расширение файла по умолчанию
            dlg.Filter = "Фото (.png)|*.png";
            Nullable<bool> result = dlg.ShowDialog();
            if (result==true)
            {
                //открываем документ
                Photo.Source = new BitmapImage(new Uri(dlg.FileName));
                DBManager.file = dlg.FileName;
            }
        }

        private void ButtonUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            //проверяем заполненность полей
            if (TextboxLogin.Text.Length!=0 && Password.Password.Length!=0 && TextboxName.Text.Length!=0 
                && ComboboxRole.SelectedIndex!=0)
            {
                manager.UpdateUser(TextboxLogin.Text, Password.Password, TextboxName.Text, ComboboxRole.SelectedIndex,
                    DBManager.file);
                MessageBox.Show("Пользователь успешно изменен");
                FrameManager.MainFrame.GoBack();
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
