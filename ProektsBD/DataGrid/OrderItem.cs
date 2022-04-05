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
        public int IdOrder { get; set; }
        public string Text { get; set; }
        public string NameOrder { get; set; }
        public string NameStatus { get; set; }
        public string NameUser { get; set; }
        public Order Order { get; set; }
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
