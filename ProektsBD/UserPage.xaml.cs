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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        DBManager manager = new DBManager();
        
        public UserPage()
        {
            InitializeComponent();
            // очищаем таблицы
            ListUser.ItemsSource = null;
            ListUser.Items.Clear();
            ListOrder.ItemsSource = null;
            ListOrder.Items.Clear();
            
            if (DBManager.UserRole==1) //проверяем что входит admin
            {
                //показываем вкладку пользователя и колонку заказчика
                UserTab.Visibility = Visibility.Visible;
                Customer.Visibility = Visibility.Visible;
                //получаем список пользователей и заявок
                List<Users> datausers = manager.db.Users.ToList();
                List<Order> dataorder = manager.db.Order.ToList();

                ListUser.ItemsSource = datausers;
                ListOrder.ItemsSource = dataorder;
            }
            else
            {
                
                //выводим данные текущего пользователя в таблицу заказов
                List<Order> dataorder = manager.db.Order.Where(o => o.Users == DBManager.UserId).ToList();
                ListOrder.ItemsSource = dataorder;
                MessageBox.Show(Convert.ToString(dataorder));
            }
            //обновляем таблицу
            ListUser.Items.Refresh();
            ListOrder.Items.Refresh();
            //запоняем комбобоксы на странице заявок
            ComboboxTypeSelect.ItemsSource = manager.db.TypeOrder.Select(o => o.NameOrder).ToList();
            ComboboxStatusSelect.ItemsSource = manager.db.Status.Select(o => o.NameStatus).ToList();
        }

        //кнопка обновления пользователя
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //проверяем выделение поля
            try
            {
                //сохраняем id выявленного пользователя
                int selectindex = ((Users)ListUser.SelectedItem).IdUsers;
                //передаем id в класс
                DBManager.Sel = selectindex;
                //сохраняем данные заявки
                manager.dataUser();
                //переходим на страницу редактирования
                FrameManager.MainFrame.Navigate(new UpdateUser());
            }
            catch (Exception)
            {
                MessageBox.Show("Ничего не выделено");
            }
        }

        //кнопка удаления пользователя
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            //проверяем выделение поля
            try
            {
                //выводим подтверждение удаления
                var result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                //если да, то...
                if (result == MessageBoxResult.Yes)
                {
                    //получаем id выбранного в таблице пользователя
                    int selected = ((Users)ListUser.SelectedItem).IdUsers;
                    //очищаем таблицу от данных
                    ListUser.ItemsSource = null;
                    ListUser.Items.Clear();
                    //удаляем
                    manager.removeUser(selected);
                    //обновляем данные в таблице
                    ListUser.ItemsSource = manager.db.Users.ToList();
                    ListUser.Items.Refresh();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ничего не выделено");
            }
        }

      
        // кнопка создания заявки
        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            //переход на страницу создания заявки
            FrameManager.MainFrame.Navigate(new AddOrder());
        }

        //кнопка обновления заявки
        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            //проверяем выделение поля
            try
            {
                //сохраняем id выявленной заявки
                int selectindex=((Order)ListOrder.SelectedItem).IdOrder;
                //передаем id в класс
                DBManager.Sel = selectindex;
                //сохраняем данные заявки
                manager.dataOrder();
                //переходим на страницу редактирования
                FrameManager.MainFrame.Navigate(new UpdateOrder());
            }
            catch(Exception)
            {
                MessageBox.Show("Ничего не выделено");
            }
        }

        //кнопка удаления заявки
        private void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            //проверяем выделение поля
            try
            {
                //выводим подтверждение удаления
                var result = MessageBox.Show("Вы действительно хотите удалить?", "Подтверждение", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                //если да, то...
                if (result == MessageBoxResult.Yes)
                {
                    //получаем id выбранного в таблице пользователя
                    int selected = ((Order)ListOrder.SelectedItem).IdOrder;
                    //очищаем таблицу от данных
                    ListOrder.ItemsSource = null;
                    ListOrder.Items.Clear();
                    //удаляем
                    manager.removeOrder(selected);
                    //обновляем данные в таблице
                    ListOrder.ItemsSource = manager.db.Order.ToList();
                    ListOrder.Items.Refresh();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ничего не выделено");
            }
        }

        //фильтрация заявок пользователя
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {   List<Order> datao=new List<Order>();//очищаем таблицу от данных
            //проверяем что в комбобоксах что-нибудь выбрано
            if (ComboboxStatusSelect.SelectedIndex>0 || ComboboxTypeSelect.SelectedIndex>0)
            {   ListOrder.ItemsSource = null;
                ListOrder.Items.Clear();
                //проверяем что оба комбобокса заполнены
                if (ComboboxStatusSelect.SelectedIndex > 0 && ComboboxTypeSelect.SelectedIndex > 0)
                {   if (DBManager.UserRole == 1)
                        datao = manager.db.Order.Where(o => o.Status == ComboboxStatusSelect.SelectedIndex
                              && o.Type == ComboboxTypeSelect.SelectedIndex).ToList();
                    else
                        datao = manager.db.Order.Where(o => o.Users == DBManager.UserId &&
                              o.Status == ComboboxStatusSelect.SelectedIndex
                              && o.Type == ComboboxTypeSelect.SelectedIndex).ToList();
                }
                //проверяем что заполнен комбобокс типа
                else if (ComboboxTypeSelect.SelectedIndex>0)
                {   if (DBManager.UserRole == 1)
                        datao = manager.db.Order.Where(o =>o.Type == ComboboxTypeSelect.SelectedIndex).
                                ToList();
                    else
                        datao = manager.db.Order.Where(o => o.Users == DBManager.UserId &&
                              o.Type == ComboboxTypeSelect.SelectedIndex).ToList();
                }
                //проверяем что заполнен комбобокс статуса
                else if (ComboboxStatusSelect.SelectedIndex>0)
                {   if (DBManager.UserRole == 1)
                        datao = manager.db.Order.Where(o => o.Status == ComboboxStatusSelect.SelectedIndex).
                            ToList();
                    else
                        datao = manager.db.Order.Where(o => o.Users == DBManager.UserId &&
                              o.Status == ComboboxStatusSelect.SelectedIndex).ToList();
                }
            }             
            else//если ни один комбобокс не заполнен
            {   if (DBManager.UserRole == 1)
                    datao = manager.db.Order.ToList();
                else
                    datao = manager.db.Order.Where(o => o.Users == DBManager.UserId).ToList();
            }
            ListOrder.ItemsSource = datao;
            ListOrder.Items.Refresh();
        }
    }
}
