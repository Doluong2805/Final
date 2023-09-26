namespace Final.Models
{
    public class I_BI_ViewModel
    {
        public List<Item>? listItem { get; set; }
        public List<BorrowItemViewModel>? borrowItemVM { get; set; }
        public int SelectedBorrowerId { get; set; }
    }
}
