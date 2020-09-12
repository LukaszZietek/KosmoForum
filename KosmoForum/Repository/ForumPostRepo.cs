﻿using System;
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
                .Include(x => x.Images).FirstOrDefault(x => x.Id == id);
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
            return _db.ForumPosts.OrderBy(x => x.Title).ToList();
        }

        public ICollection<ForumPost> GetAllForumPostsInCategory(int categoryId)
        {
            var values = _db.ForumPosts.Where(x => x.CategoryId == categoryId).ToList();
            return values;
        }

        public ICollection<ForumPost> GetAllForumPostsForUser(int userId)
        {
            var values = _db.ForumPosts.Where(x => x.UserId == userId).ToList();
            return values;
        }

        public bool CreateForumPost(ForumPost obj)
        {
            _db.ForumPosts.Add(obj);
            return Save();
        }

        public bool DeleteForumPost(ForumPost obj)
        {
            _db.ForumPosts.Remove(obj);
            return Save();
        }

        public bool UpdateForumPost(ForumPost obj)
        {
            _db.ForumPosts.Update(obj);
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