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
            DBManager.GetTypeNameOrders().ForEach(x => ComboboxTypeOrder.Items.Add(x));
            DBManager.GetStatusNameOrders().ForEach(x => ComboboxStatusOrder.Items.Add(x));
            // Выбор актуальных данных для заказа
            ComboboxTypeOrder.SelectedIndex = order.IdType - 1;
            ComboboxStatusOrder.SelectedIndex = order.IdStatus - 1;
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            string text = TextboxUpdateOrder.Text;
            TypeOrder typeOrder = ComboboxTypeOrder.SelectedItem as TypeOrder;
            Status status = ComboboxStatusOrder.SelectedItem as Status;

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Запонмите все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Order.Text = text;
            Order.TypeOrder = typeOrder;
            Order.Status = status;
            DBManager.db.SaveChanges();
            Utils.UserWindow.UpdateListOrder();
            MessageBox.Show("Заявка успешно отредактирована!", "Успешно" ,MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
