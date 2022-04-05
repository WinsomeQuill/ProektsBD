using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public OrderItem(Order order)
        {
            IdOrder = order.IdOrder;
            Text = order.Text;
            NameOrder = order.TypeOrder.NameOrder;
            NameStatus = order.Status.NameStatus;
            NameUser = order.Users.NameUser;
            Order = order;
        }
    }
}
