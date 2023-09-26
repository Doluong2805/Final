using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class BorrowItemViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        [Range(1, int.MaxValue)]

        [DisplayName("Quantity")]
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Cost")]
        public decimal Cost { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        public int? MaxQuantityFromItem { get; set; }
    }
}
