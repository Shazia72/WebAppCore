using System;
using System.Collections.Generic;

namespace WebAppCore1.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
