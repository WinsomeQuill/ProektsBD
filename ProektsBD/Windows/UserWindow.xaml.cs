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
                UpdateListUsers();
            }
            else
            {
                UserTab.Visibility = Visibility.Collapsed;
            }


            ComboboxTypeSelect.Items.Add(new ComboBoxItem { Content = "Все" });
            ComboboxStatusSelect.Items.Add(new ComboBoxItem { Content = "Все" });

            DBManager.GetTypeOrders().ForEach(x =>
            {
                ComboboxTypeSelect.Items.Add(new ComboBoxItem { Content = x.NameOrder, Tag = x });
            });

            DBManager.GetStatusOrders().ForEach(x =>
            {
                ComboboxStatusSelect.Items.Add(new ComboBoxItem { Content = x.NameStatus, Tag = x });
            });
            ComboboxStatusSelect.SelectedIndex = ComboboxTypeSelect.SelectedIndex = 0;

            UpdateListOrder();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            UpdateListOrder();
        }

        private void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.OrderItem orderItem = ListOrder.SelectedItem as DataGrid.OrderItem;
            if (orderItem == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            int selected = orderItem.IdOrder;
            ListOrder.Items.Clear();
            DBManager.RemoveOrder(selected);
            UpdateListOrder();
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.OrderItem orderItem = ListOrder.SelectedItem as DataGrid.OrderItem;
            if (orderItem == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new UpdateOrderWindow(orderItem.Order).ShowDialog();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            new AddOrderWindow().ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.UserItem userItem = ListUser.SelectedItem as DataGrid.UserItem;
            if (userItem == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new UpdateUserWindow(userItem.User).ShowDialog();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

            DataGrid.UserItem userItem = ListUser.SelectedItem as DataGrid.UserItem;
            if (userItem == null)
            {
                MessageBox.Show("Ничего не выделено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (userItem.User.IdUsers == UserCache.Id)
            {
                MessageBox.Show("Нельзя удалить самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            int selected = userItem.IdUsers;
            DBManager.RemoveUser(selected);
            UpdateListUsers();
            UpdateListOrder();
        }

        private void ButtonGoBack_Click(object sender, RoutedEventArgs e)
        {
            UserCache.Reset();
            new AuthorizationWindow().Show();
            Close();
        }

        public void UpdateListOrder()
        {
            ListOrder.Items.Clear();
            List<Order> datao = new List<Order>(); // создаем список

            ComboBoxItem itemStatus = ComboboxStatusSelect.SelectedItem as ComboBoxItem;
            ComboBoxItem itemType = ComboboxTypeSelect.SelectedItem as ComboBoxItem;

            int statusId = 0, typeId = 0;

            if (itemStatus.Tag != null)
            {
                // получение id статуса
                statusId = (itemStatus.Tag as Status).IdStatus;
            }

            if (itemType.Tag != null)
            {
                // получение id типа
                typeId = (itemType.Tag as TypeOrder).IdTypeOrder;
            }

            if (UserCache.Role.IdRole == 1) // если авторизовался администратор, то все заказы
            {
                // если выбраны все типы и все статусы
                if (itemStatus.Content.ToString() == "Все" && itemType.Content.ToString() == "Все")
                {
                    datao = DBManager.db.Order.ToList();
                }
                // если выбраны все статусы
                if (itemStatus.Content.ToString() == "Все" && itemType.Content.ToString() != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == typeId).ToList();
                }
                // если выбраны все типы
                if (itemStatus.Content.ToString() != "Все" && itemType.Content.ToString() == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdStatus == statusId).ToList();
                }
                // если выбраны различные типы и статусы
                if (itemStatus.Content.ToString() != "Все" && itemType.Content.ToString() != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == typeId && x.IdStatus == statusId).ToList();
                }
            }
            else // если авторизовался пользователь, то отображаем только его заказы 
            {
                // если выбраны все типы и все статусы
                if (itemStatus.Content.ToString() == "Все" && itemType.Content.ToString() == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны все статусы
                if (itemStatus.Content.ToString() == "Все" && itemType.Content.ToString() != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == typeId && x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны все типы
                if (itemStatus.Content.ToString() != "Все" && itemType.Content.ToString() == "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdStatus == statusId && x.IdUsers == UserCache.Id).ToList();
                }
                // если выбраны различные типы и статусы
                if (itemStatus.Content.ToString() != "Все" && itemType.Content.ToString() != "Все")
                {
                    datao = DBManager.db.Order.Where(x => x.IdType == typeId && x.IdStatus == statusId && x.IdUsers == UserCache.Id).ToList();
                }
            }

            datao.ForEach(x =>
            {
                ListOrder.Items.Add(new DataGrid.OrderItem(x));
            });
        }

        public void UpdateListUsers()
        {
            ListUser.Items.Clear();
            DBManager.GetUsers().ForEach(x =>
            {
                ListUser.Items.Add(new DataGrid.UserItem(x));
            });
        }
    }
}
