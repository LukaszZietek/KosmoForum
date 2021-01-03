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
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<Category> GetAllCategories()
        {
            var obj = _db.Categories.OrderBy(x => x.Title).ToList();
            return obj.Count > 0 ? obj : null;
        }

        public Category GetCategory(int id)
        {
            return _db.Categories.FirstOrDefault(x => x.Id == id);
        }

        public Category GetCategory(string title)
        {
            var obj = _db.Categories.FirstOrDefault(x => x.Title.Trim().ToLower() == title.Trim().ToLower());
            return obj;

        }

        public bool CategoryExists(int id)
        {
            var value = _db.Categories.Any(x => x.Id == id);
            return value;
        }

        public bool CategoryExists(string name)
        {
            var value = _db.Categories.Any(x => x.Title.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool CreateCategory(Category category)
        {
            if (category == null)
            {
                return false;
            }

            _db.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _db.Categories.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            var obj = _db.ForumPosts.Include(x => x.Opinions).Where(x => x.CategoryId == category.Id).ToList();

            foreach (var item in obj)
            {
                _db.ForumPosts.Remove(item);
            }

            _db.Categories.Remove(category);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
