using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;

namespace KosmoForum.Repository.IRepository
{
    public interface IForumPostRepo
    {
        ForumPost GetPost(int id);
        ForumPost GetPost(string title);
        ICollection<ForumPost> GetAllPosts();
        ICollection<ForumPost> GetAllForumPostsInCategory(int categoryId);

        ICollection<ForumPost> GetAllForumPostsForUser(int userId);

        bool CreateForumPost(ForumPost obj);

        bool DeleteForumPost(ForumPost obj);

        bool UpdateForumPost(ForumPost obj);

        bool ForumPostIfExist(int id);
        bool ForumPostIfExist(string title);
        bool Save();

    }
}
