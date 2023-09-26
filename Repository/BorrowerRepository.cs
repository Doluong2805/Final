using Final.Data;
using Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Final.Repository
{
    public class BorrowerRepository : IGenericRepository<Borrower>
    {
        private readonly ApplicationDbContext _context;
        public BorrowerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Borrower borrower)
        {
            _context.Add(borrower);
            _context.SaveChanges();
        }
        public void Update(Borrower borrower)
        {
            _context.Update(borrower);
            _context.SaveChanges();
        }
        public void Delete(Borrower borrower)
        {
            _context.Remove(borrower);
            _context.SaveChanges();
        }
        public Borrower? GetById(int id)
        {
            return _context.Borrowers
                .Include(b => b.histories)
                .FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<Borrower> GetAll()
        {
            return _context.Borrowers;
        }
        public async Task CreateAsync(Borrower entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<Borrower?> GetByIdAsync(int id)
        {
            return await _context.Borrowers.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
