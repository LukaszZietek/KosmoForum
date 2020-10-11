using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;

namespace KosmoForum.Repository.IRepository
{
    public interface IUserRepo
    {
        bool IsUniqueUser(string username);

        User GetUser(int userId);

        User Authenticate(string username, string password);

        User Register(string username, string password, string email, byte[] avatar);

        int GetUserIdUsingName(string username);

        byte[] GetUserAvatar(int userId);

        bool UpdateUser(User userObj);

        bool Save();
    }
}
