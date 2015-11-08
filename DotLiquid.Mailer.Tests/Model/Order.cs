using System.Collections.Generic;

namespace DotLiquid.Mailer.Tests.Model
{

    public class Order
    {
        public Customer Cust { get; set; }
        public List<Item> Itemlist { get; set; }

        public Order()
        {
            Itemlist = new List<Item>();
        } 
    }

}