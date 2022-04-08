using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProektsBD.DataGrid
{
    public class OrderItem
    {
        public int IdOrder { get; set; } // id заявки
        public string Text { get; set; } // описание заявки
        public string NameOrder { get; set; } // назание заявки
        public string NameStatus { get; set; } // статус заявки
        public string NameUser { get; set; } // имя пользоателя
        public Order Order { get; set; } // заявка
        public BitmapImage Photo { get; set; } // фотография заказчика

        public OrderItem(Order order)
        {
            IdOrder = order.IdOrder;
            Text = order.Text;
            NameOrder = order.TypeOrder.NameOrder;
            NameStatus = order.Status.NameStatus;
            NameUser = order.Users.NameUser;
            Order = order;
            Photo = Utils.BinaryToImage(order.Users.Photo);
        }
    }
}
