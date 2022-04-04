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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        DBManager manager = new DBManager();

        public AddOrder()
        {
            InitializeComponent();
            ComboboxTypeOrder.ItemsSource = manager.db.TypeOrder.Select(o => o.NameOrder).ToList();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (TextboxAddOrder.Text.Length != 0 && ComboboxTypeOrder.SelectedIndex != 0)
            {
                manager.AddOrder(TextboxAddOrder.Text, ComboboxTypeOrder.SelectedIndex);
                MessageBox.Show("Заявка успешно создана");
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
