using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebAppCore1.ViewModels
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Required]
        [DisplayName("Item Code")]
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public IFormFile ImagePath { get; set; }
        public decimal ItemPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
