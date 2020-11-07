using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IRepository<T> where T: class
    {
        Task<Tuple<string,T>> GetAsync(string url, int id, string token);
        Task<Tuple<string,IEnumerable<T>>> GetAllAsync(string url, string token);

        Task<Tuple<string,bool>> UpdateAsync(string url, int id, T obj, string token);

        Task<Tuple<string,bool>> DeleteAsync(string url, int id, string token);

        Task<Tuple<string,bool>> CreateAsync(string url, T obj, string token);

    }
}
