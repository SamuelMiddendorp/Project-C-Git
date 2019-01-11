using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime orderDate { get; set; }
    }
}
