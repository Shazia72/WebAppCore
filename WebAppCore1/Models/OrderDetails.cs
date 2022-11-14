using System;
using System.Collections.Generic;

namespace WebAppCore1.Models
{
    public partial class OrderDetails
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Items Item { get; set; }
        public virtual Orders Order { get; set; }
    }
}
