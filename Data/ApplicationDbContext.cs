using Final.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Item { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<BorrowItem> BorrowedItems { get; set; }
        public DbSet<History> History{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<History>()
                .HasOne(h => h.Borrower) // Một BorrowHistory có một Borrower
                .WithMany(b => b.histories) // Một Borrower có nhiều BorrowHistory
                .HasForeignKey(h => h.BorrowerId) // Khóa ngoại BorrowerId
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowItem>()
                .HasOne(bi => bi.Item) // Một BorrowedItem có một Item
                .WithMany(i => i.BorrowItems) // Một Item có nhiều BorrowedItem
                .HasForeignKey(bi => bi.ItemId) // Khóa ngoại ItemId
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowItem>()
                .HasOne(bi => bi.History) // Một BorrowedItem có một BorrowHistory
                .WithMany(h => h.BorrowItems) // Một BorrowHistory có nhiều BorrowedItem
                .HasForeignKey(bi => bi.HistoryId) // Khóa ngoại BorrowHistoryId
                .OnDelete(DeleteBehavior.Cascade); // Xóa BorrowedItem khi BorrowHistory bị xóa
        }

    }
}