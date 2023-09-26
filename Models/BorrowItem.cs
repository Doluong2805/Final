using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class BorrowItem : BaseEntity
    {
        public int HistoryId { get; set; }
        [Display(Name = "Title")]
        public int ItemId { get; set; }
        [Required]
        [Range(1,1000)]
        public int Quantity { get; set; }
        public int ReturnedQuanyity { get; set; }
        [Required]
        [Range(1,1000)]
        public decimal Cost { get; set; }
        public Item? Item { get; set; }
        public History? History { get; set; }
    }
}
