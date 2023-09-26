using Final.Data;
using Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Final.Repository
{
    public class HistoryRepository : IGenericRepository<History>
    {
        private readonly ApplicationDbContext _context;
        public HistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(History entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task CreateAsync(History entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(History entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<History> GetAll()
        {
            return _context.History;
        }

        public History? GetById(int id)
        {
            return _context.History.FirstOrDefault(x => x.Id == id);
        }

        public async Task<History?> GetByIdAsync(int id)
        {
            return await _context.History.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(History entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
