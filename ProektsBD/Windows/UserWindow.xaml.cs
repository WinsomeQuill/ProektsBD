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

            Utils.UserWindow = this;

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

            ComboboxTypeSelect.Items.Add(new TypeOrder() { NameOrder = "Все" });
            ComboboxStatusSelect.Items.Add(new Status() { NameStatus = "Все" });

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
            UpdateListOrder();
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
            foreach (Order item in orders)
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

            new UpdateOrderWindow(order).ShowDialog();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            new AddOrderWindow().ShowDialog();
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
            foreach (Users item in users)
            {
                ListUser.Items.Add(item);
            }
        }

        public void UpdateListOrder()
        {
            List<Order> datao = new List<Order>(); // создаем список
            // получение id статуса
            int StatusId = (ComboboxStatusSelect.SelectedItem as Status).IdStatus;
            // получение id типа
            int TypeId = (ComboboxTypeSelect.SelectedItem as TypeOrder).IdTypeOrder;

            if (UserCache.Id == 1) // если авторизовался администратор, то все заказы
            {
                // если выбраны все типы и все статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus == "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder == "Все")
                {
                    datao = DBManager.db.Order.ToList();
                }
                // если выбраны все статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus == "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == TypeId).ToList();
                }
                // если выбраны все типы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus != "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdStatus == StatusId).ToList();
                }
                // если выбраны различные типы и статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus != "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == TypeId && x.IdStatus == StatusId).ToList();
                }
            }
            else // если авторизовался пользователь, то отображаем только его заказы 
            {
                // если выбраны все типы и все статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus == "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны все статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus == "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == TypeId && x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны все типы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus != "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdStatus == StatusId && x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны различные типы и статусы
                if ((ComboboxStatusSelect.SelectedItem as Status).NameStatus != "Все" && (ComboboxTypeSelect.SelectedItem as TypeOrder).NameOrder != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == TypeId && x.IdStatus == StatusId && x.IdUsers == UserCache.Id).ToList();
                }
            }

            foreach (Order item in datao)
            {
                ListOrder.Items.Add(item);
            }
        }
    }
}
