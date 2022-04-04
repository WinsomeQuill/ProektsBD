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
    /// Логика взаимодействия для UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : Page
    {
        DBManager manager = new DBManager();

        public UpdateOrder()
        {
            InitializeComponent();
            ComboboxTypeOrder.ItemsSource = manager.db.TypeOrder.Select(o => o.NameOrder).ToList();
            ComboboxTypeOrder.SelectedIndex = DBManager.Type;
            ComboboxStatusOrder.ItemsSource = manager.db.Status.Select(o => o.NameStatus).ToList();
            ComboboxStatusOrder.SelectedIndex = DBManager.Status;
            TextboxUpdateOrder.Text = DBManager.Text;
            
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (TextboxUpdateOrder.Text.Length!=0 && ComboboxTypeOrder.SelectedIndex!=0 && 
                ComboboxStatusOrder.SelectedIndex!=0)
            {
                manager.UpdateOrder(TextboxUpdateOrder.Text, ComboboxTypeOrder.SelectedIndex, 
                    ComboboxStatusOrder.SelectedIndex);
                MessageBox.Show("Заявка успешно отредактирована");
                FrameManager.MainFrame.GoBack();
            }
            else
            {
                MessageBox.Show("Запонмите все поля");
            }
        }
    }
}
