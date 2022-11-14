using System;
using System.Collections.Generic;

namespace WebAppCore1.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Items = new HashSet<Items>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
