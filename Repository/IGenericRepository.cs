using Final.Models;
using System.Linq.Expressions;

namespace Final.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(int id);
        IQueryable<T> GetAll();

        Task CreateAsync(T entity);
        Task<T?> GetByIdAsync(int id);
    }
}
