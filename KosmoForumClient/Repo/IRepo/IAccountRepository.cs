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
        Task<Tuple<string, User>> LoginAsync(string url, User objToCreate);

        Task<Tuple<string,bool>> RegisterAsync(string url, User objToCreate);

        Task<Tuple<string,int>> GetUserId(string url, string username, string token);

        Task<Tuple<string,byte[]>> GetUserAvatar(string url,string token);

        Task<Tuple<string,bool>> UpdateUserAvatar(string url, byte[] avatar, string token);

        Task<Tuple<string,bool>> ChangePassword(string url, ChangePasswordVM passwordVM, string token);
    }
}
