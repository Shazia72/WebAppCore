using System;
using System.Collections.Generic;

namespace WebAppCore1.Models
{
    public partial class Items
    {
        public Items()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal ItemPrice { get; set; }
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
