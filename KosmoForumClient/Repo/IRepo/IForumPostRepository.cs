using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IForumPostRepository : IRepository<ForumPost>
    {
        Task<IEnumerable<ForumPost>> GetAllFromCategory(string url,int categoryId);
    }
}
