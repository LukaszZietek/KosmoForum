using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);

        Task<bool> UpdateAsync(string url, int id, T obj);

        Task<bool> DeleteAsync(string url, int id);

        Task<bool> CreateAsync(string url, T obj);

    }
}
