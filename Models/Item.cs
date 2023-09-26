using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Final.Models
{
    public enum ItemType
    {
        Book,
        DVD,
        Magazine
    }
    public class Item : BaseEntity
    {
        [EnumDataType(typeof(ItemType))]
        public ItemType Type { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Author { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
        [Required]
        [Range(1,1000)]
        public decimal Price { get; set; }
        [Required]
        [Range(1,1000)]
        public int Quantity { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        [Range(1,1000)]
        public int? NumberOfPages { get; set; }
        [Range(1,1000)]
        public int? RunTime { get; set; }
        public List<BorrowItem>? BorrowItems { get; set; }
    }
}
