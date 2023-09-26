using Final.Data;
using Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Final.Repository
{
    public class BorrowItemRepository : IGenericRepository<BorrowItem>
    {
        private readonly ApplicationDbContext _context;
        public BorrowItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(BorrowItem entity)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(BorrowItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BorrowItem entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<BorrowItem> GetAll()
        {
            return _context.BorrowedItems;
        }

        public BorrowItem GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BorrowItem?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BorrowItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
