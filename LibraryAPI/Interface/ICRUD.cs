using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Interface
{
    public interface ICRUD<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task Delete(int id);
        Task<T> GetById(int id);
        


    }
}
