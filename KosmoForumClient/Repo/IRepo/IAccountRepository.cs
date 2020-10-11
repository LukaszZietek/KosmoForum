using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);

        Task<bool> RegisterAsync(string url, User objToCreate);

        Task<int> GetUserId(string url, string username, string token);

        Task<byte[]> GetUserAvatar(string url,string token);

        Task<bool> UpdateUserAvatar(string url, byte[] avatar, string token);

        Task<bool> ChangePassword(string url, ChangePasswordVM passwordVM, string token);
    }
}
