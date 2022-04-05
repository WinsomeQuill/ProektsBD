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
    /// Логика взаимодействия для UpdateOrderWindow.xaml
    /// </summary>
    public partial class UpdateOrderWindow : Window
    {
        private Order Order { get; }
        public UpdateOrderWindow(Order order)
        {
            InitializeComponent();
            Order = order;
            // Вывод данных в компоненты
            TextboxUpdateOrder.Text = order.Text;
            DBManager.GetTypeOrders().ForEach(x =>
            {
                ComboBoxItem item = new ComboBoxItem { Content = x.NameOrder, Tag = x };
                ComboboxTypeOrder.Items.Add(item);
                if (x.IdTypeOrder == Order.IdType)
                {
                    ComboboxTypeOrder.SelectedItem = item;
                }
            });

            DBManager.GetStatusOrders().ForEach(x =>
            {
                ComboBoxItem item = new ComboBoxItem { Content = x.NameStatus, Tag = x };
                ComboboxStatusOrder.Items.Add(item);
                if (x.IdStatus == Order.IdStatus)
                {
                    ComboboxStatusOrder.SelectedItem = item;
                }
            });
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            string text = TextboxUpdateOrder.Text;
            ComboBoxItem typeItem = ComboboxTypeOrder.SelectedItem as ComboBoxItem;
            ComboBoxItem statusItem = ComboboxStatusOrder.SelectedItem as ComboBoxItem;

            TypeOrder typeOrder = typeItem.Tag as TypeOrder;
            Status statusOrder = statusItem.Tag as Status;

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DBManager.UpdateOrder(Order.IdOrder, text, typeOrder.IdTypeOrder, statusOrder.IdStatus);
            Utils.UserWindow.UpdateListOrder();
            MessageBox.Show("Заявка успешно отредактирована!", "Успешно" ,MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
