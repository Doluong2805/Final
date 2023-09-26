using Final.Models;

namespace Final.Service
{
    public interface IBorrowerServices
    {
        IQueryable<Borrower> GetAll();
        Borrower GetById(int id);
        void Create(Borrower br);
        void Update(Borrower br);
        void Delete(int id);
        IQueryable<Borrower> Search(IQueryable<Borrower> query, string? name);
        IQueryable<Borrower> Pagination(IQueryable<Borrower> query,int pageNumber,int pageSize);
        Task<Borrower?> GetByIdAsync(int id);
        Task CreateAsync(Borrower br);
        int CountPage(IQueryable<Borrower> query, int pageSize);
    }
}
