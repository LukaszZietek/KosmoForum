using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.DbContext;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace KosmoForum.Repository
{
    public class ForumPostRepo : IForumPostRepo
    {
        private readonly ApplicationDbContext _db;

        public ForumPostRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public ForumPost GetPost(int id)
        {
            var value = _db.ForumPosts.Include(x => x.Opinions)
                .Include(x => x.Images).Include(x => x.User).
                FirstOrDefault(x => x.Id == id);
            return value;
        }

        public ForumPost GetPost(string title)
        {
            var value = _db.ForumPosts.Include(x => x.Opinions).Include(x => x.Images)
                .FirstOrDefault(x => x.Title.Trim().ToLower() == title.Trim().ToLower());
            return value;
        }

        public ICollection<ForumPost> GetAllPosts()
        {
            return _db.ForumPosts.Include(x => x.User)
                .OrderBy(x => x.Title).ToList();
        }

        public ICollection<ForumPost> GetAllForumPostsInCategory(int categoryId)
        {
            var values = _db.ForumPosts.Include(x => x.User).Where(x => x.CategoryId == categoryId).ToList();
            return values;
        }

        public ICollection<ForumPost> GetAllForumPostsForUser(int userId)
        {
            var values = _db.ForumPosts
                .Include( x => x.User)
                .Where(x => x.UserId == userId).OrderBy(x => x.Date).ToList();
            return values;
        }

        public bool CreateForumPost(ForumPost obj)
        {
            if (obj.Images.Count > 0)
            {
                foreach (var item in obj.Images)
                {
                    item.AddTime = DateTime.Now;
                    item.ForumPost = obj;
                    _db.Images.Add(item);
                }
            }

            _db.ForumPosts.Add(obj);
            return Save();
        }

        public bool DeleteForumPost(ForumPost obj)
        {
            var obj2 = _db.ForumPosts.Include(x => x.Images).Include(x => x.Opinions)
                .FirstOrDefault(x => x.Id == obj.Id);

            _db.ForumPosts.Remove(obj2);
            return Save();
        }

        public bool UpdateForumPost(ForumPost obj)
        {

            var originalObj = _db.ForumPosts.FirstOrDefault(x => x.Id == obj.Id);
            originalObj.CategoryId = obj.CategoryId;
            originalObj.Content = obj.Content;
            originalObj.Title = obj.Title;
            originalObj.UserId = obj.UserId;
            originalObj.Date = DateTime.Now;

            foreach (var item in obj.Images)
            {
                if (!(item.Id > 0))
                {
                    item.AddTime = DateTime.Now;
                    item.UserId = obj.UserId;
                    item.ForumPost = obj;
                    originalObj.Images.Add(item);
                }
            }
            return Save();
        }

        public bool ForumPostIfExist(int id)
        {
            var value = _db.ForumPosts.Any(x => x.Id == id);
            return value;
        }

        public bool ForumPostIfExist(string title)
        {
            var value = _db.ForumPosts.Any(x => x.Title.ToLower().Trim() == title.ToLower().Trim());
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
