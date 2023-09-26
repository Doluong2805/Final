using Final.Models;
using Final.Repository;
using NuGet.Protocol.Core.Types;
using System.Text;

namespace Final.Service
{
    public class BorrowerServices : IBorrowerServices
    {
        private readonly IGenericRepository<Borrower> _brepository;

        public BorrowerServices(IGenericRepository<Borrower> brepository)
        {
            _brepository = brepository;
        }

        public void Create(Borrower br)
        {
            _brepository.Create(br);
        }

        public async Task CreateAsync(Borrower br)
        {
            await _brepository.CreateAsync(br);
        }

        public void Delete(int id)
        {
            try
            {
                var borrowedItems = _brepository.GetById(id);
                if(borrowedItems.histories != null && borrowedItems.histories.Any())
                {
                    throw new Exception("Không thể xóa khi đang có trong lịch sử!");
                }
                _brepository.Delete(_brepository.GetById(id));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<Borrower> GetAll()
        {
            return _brepository.GetAll();
        }

        public Borrower GetById(int id)
        {
            return _brepository.GetById(id);
        }

        public async Task<Borrower?> GetByIdAsync(int id)
        {
            return await _brepository.GetByIdAsync(id);
        }

        public IQueryable<Borrower> Search(IQueryable<Borrower> query, string? name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(item => item.Name.ToLower().Contains(name));
            }
            return query;
        }
        public IQueryable<Borrower> Pagination(IQueryable<Borrower> query,int pageNumber,int pageSize)
        {
            try
            {
                return query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(Borrower br)
        {
            _brepository.Update(br);
        }

        public int CountPage(IQueryable<Borrower> query, int pageSize)
        {
            try
            {
                int totalItems = query.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return totalPages;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
