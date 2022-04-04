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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();

            if (UserCache.Role.IdRole == 1) // Если админ
            {
                UserTab.Visibility = Visibility.Visible;
                Customer.Visibility = Visibility.Visible;
                List<Users> users = DBManager.GetUsers();
                foreach (Users user in users)
                {
                    ListUser.Items.Add(user);
                }

                List<Order> orders = DBManager.GetOrders();
                foreach (Order order in orders)
                {
                    ListOrder.Items.Add(order);
                }
            }
            else
            {
                UserTab.Visibility = Visibility.Collapsed;
                List<Order> orders = DBManager.db.Order.Where(o => o.IdUsers == UserCache.Id).ToList();
                foreach (Order order in orders)
                {
                    ListOrder.Items.Add(order);
                }
            }

            List<string> typeOrders = DBManager.GetTypeNameOrders();
            foreach (string typeOrder in typeOrders)
            {
                ComboboxTypeSelect.Items.Add(typeOrder);
            }

            List<string> statusOrders = DBManager.GetStatusNameOrders();
            foreach (string statusOrder in statusOrders)
            {
                ComboboxStatusSelect.Items.Add(statusOrder);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Order> datao = new List<Order>();
            //проверяем что в комбобоксах что-нибудь выбрано
            if (ComboboxStatusSelect.SelectedIndex > 0 || ComboboxTypeSelect.SelectedIndex > 0)
            {
                ListOrder.Items.Clear();
                //проверяем что оба комбобокса заполнены
                if (ComboboxStatusSelect.SelectedIndex > 0 && ComboboxTypeSelect.SelectedIndex > 0)
                {
                    if (UserCache.Role.IdRole == 1)
                    {
                        datao = DBManager.db.Order.Where(o => o.IdStatus == ComboboxStatusSelect.SelectedIndex
                              && o.IdType == ComboboxTypeSelect.SelectedIndex).ToList();
                    }
                    else
                    {
                        datao = DBManager.db.Order.Where(o => o.IdUsers == UserCache.Role.IdRole &&
                              o.IdStatus == ComboboxStatusSelect.SelectedIndex
                              && o.IdType == ComboboxTypeSelect.SelectedIndex).ToList();
                    }
                }
                //проверяем что заполнен комбобокс типа
                else if (ComboboxTypeSelect.SelectedIndex > 0)
                {
                    if (UserCache.Role.IdRole == 1)
                    {
                        datao = DBManager.db.Order.Where(o => o.IdType == ComboboxTypeSelect.SelectedIndex)
                            .ToList();
                    }
                    else
                    {
                        datao = DBManager.db.Order.Where(o => o.IdUsers == UserCache.Id &&
                              o.IdType == ComboboxTypeSelect.SelectedIndex).ToList();
                    }
                }
                //проверяем что заполнен комбобокс статуса
                else if (ComboboxStatusSelect.SelectedIndex > 0)
                {
                    if (UserCache.Role.IdRole == 1)
                    {
                        datao = DBManager.db.Order.Where(o => o.IdStatus == ComboboxStatusSelect.SelectedIndex)
                            .ToList();
                    }
                    else
                    {
                        datao = DBManager.db.Order.Where(o => o.IdUsers == UserCache.Id &&
                              o.IdStatus == ComboboxStatusSelect.SelectedIndex).ToList();
                    }
                }
            }
            else //если ни один комбобокс не заполнен
            {
                if (UserCache.Role.IdRole == 1)
                {
                    datao = DBManager.db.Order.ToList();
                }
                else
                {
                    datao = DBManager.db.Order.Where(o => o.IdUsers == UserCache.Id).ToList();
                }
            }

            foreach (Order item in datao)
            {
                ListOrder.Items.Add(item);
            }
        }

        private void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = ListOrder.SelectedItem as Order;
            if (order == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            int selected = order.IdOrder;
            ListOrder.Items.Clear();
            DBManager.RemoveOrder(selected);

            List<Order> orders = DBManager.GetOrders();
            foreach(Order item in orders)
            {
                ListOrder.Items.Add(item);
            }
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = ListOrder.SelectedItem as Order;
            if (order == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectindex = order.IdOrder;
            FrameManager.MainFrame.Navigate(new UpdateOrder());
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            // pass
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Users user = ListUser.SelectedItem as Users;
            if (user == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectindex = user.IdUsers;
            FrameManager.MainFrame.Navigate(new UpdateUser());
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

            Users user = ListUser.SelectedItem as Users;
            if (user == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            int selected = user.IdUsers;
            ListUser.Items.Clear();
            DBManager.RemoveUser(selected);

            List<Users> users = DBManager.GetUsers();
            foreach(Users item in users)
            {
                ListUser.Items.Add(item);
            }
        }
    }
}
