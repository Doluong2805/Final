using Final.Data;
using Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Final.Repository
{
    public class ItemRepository : IGenericRepository<Item>
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Item entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public void Update(Item entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(Item entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        public Item? GetById(int id)
        {
            return _context.Item
                .Include(x => x.BorrowItems)
                .FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<Item> GetAll()
        {
            return _context.Item
                .Include(i => i.BorrowItems);
        }

        public async Task CreateAsync(Item entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Item.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
