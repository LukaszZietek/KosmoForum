using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);

        Task<bool> RegisterAsync(string url, User objToCreate);
    }
}
