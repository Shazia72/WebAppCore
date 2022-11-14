using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppCore1.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int ItemId { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Required]
        [DisplayName("Item Code")]
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal ItemPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
