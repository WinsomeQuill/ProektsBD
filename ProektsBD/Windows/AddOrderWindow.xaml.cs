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
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            DBManager.GetTypeOrders().ForEach(x => // добаление типов заявок в ComboBox
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = x.NameOrder,
                    Tag = x
                };
                ComboboxTypeOrder.Items.Add(comboBoxItem);
            });
            ComboboxTypeOrder.SelectedIndex = 0;
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            string text = TextboxAddOrder.Text;

            if (text.Length == 0)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ComboBoxItem comboBoxItem = ComboboxTypeOrder.SelectedItem as ComboBoxItem;
            TypeOrder typeOrder = comboBoxItem.Tag as TypeOrder;

            DBManager.AddOrder(text, typeOrder.IdTypeOrder);
            Utils.UserWindow.UpdateListOrder();
            MessageBox.Show("Вы создали заявку!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
