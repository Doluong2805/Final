using Final.Models;

namespace Final.Service
{
    public interface IItemServices
    {
        IQueryable<Item> GetAll();
        Item GetById(int id);
        void Create(Item item);
        void Update(Item item);
        void Delete(int id);
        IQueryable<Item> Search(IQueryable<Item> query, string? title);
        IQueryable<Item> Pagination(IQueryable<Item> query,int pageNumber,int pageSize);
        Task<Item?> GetByIdAsync(int id);
        Task CreateAsync(Item item);
        int CountPage(IQueryable<Item> query, int pageSize);
    }
}
