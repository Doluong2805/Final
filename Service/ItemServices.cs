using Final.Models;
using Final.Repository;
using Microsoft.EntityFrameworkCore;

namespace Final.Service
{
    public class ItemServices : IItemServices
    {
        private readonly IGenericRepository<Item> _repository;
        public ItemServices(IGenericRepository<Item> repository)
        {
            _repository = repository;
        }

        public IQueryable<Item> GetAll()
        {
            return _repository.GetAll();
        }

        public Item GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Create(Item item)
        {
            _repository.Create(item);
        }

        public void Update(Item item)
        {
            _repository.Update(item);
        }

        public void Delete(int id)
        {
            try
            {
                var borrowedItems = _repository.GetById(id);
                if(borrowedItems.BorrowItems != null && borrowedItems.BorrowItems.Any())
                {
                    throw new Exception("Không thể xóa khi đang có trong lịch sử!");
                }
                _repository.Delete(_repository.GetById(id));
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<Item> Pagination(IQueryable<Item> query,int pageNumber,int pageSize)
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

        public IQueryable<Item> Search(IQueryable<Item> query,string? title)
        {
            if( title != null)
            {
                query = query.Where(item => item.Title.ToLower().Contains(title));
            }
            return query;
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Item item)
        {
            await _repository.CreateAsync(item);
        }

        public int CountPage(IQueryable<Item> query, int pageSize)
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
