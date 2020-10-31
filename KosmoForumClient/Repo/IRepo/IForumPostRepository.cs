using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IForumPostRepository : IRepository<ForumPost>
    {
        Task<Tuple<string,IEnumerable<ForumPost>>> GetAllFromCategory(string url,int categoryId, string token);

        Task<Tuple<string,IEnumerable<ForumPost>>> GetAllBelongsToUser(string url, string token);
    }
}
