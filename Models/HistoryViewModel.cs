namespace Final.Models
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Status Status { get; set; }
        public List<BorrowItem>? BorrowItems { get; set; }
    }
}
