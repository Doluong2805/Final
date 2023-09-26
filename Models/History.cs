using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public enum Status
    {
        Opened,
        Closed
    }
    public class History : BaseEntity
    {
        public int BorrowerId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }
        public Status Status { get; set; }
        public Borrower? Borrower { get; set; }
        public List<BorrowItem>? BorrowItems { get; set; }
    }
}
