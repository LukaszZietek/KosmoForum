using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;

namespace KosmoForumTests.FakeRepo
{
    class CategoryFakeRepo : ICategoryRepo
    {
        private readonly List<Category> _categoriesList;

        public CategoryFakeRepo()
        {
            _categoriesList = new List<Category>()
            {
                new Category() {CreationDateTime = DateTime.Now, Description = "Ala ma kota", Id = 0, Title = "Włosy"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Kot ma ale", Id = 1, Title = "Głowa"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Transformers", Id = 2, Title = "Nogi"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Czara ognia", Id = 3, Title = "Uszy"},
            };
        }

        public ICollection<Category> GetAllCategories()
        {
            return _categoriesList.OrderBy(x => x.Title).ToList();
        }

        public Category GetCategory(int id)
        {
            return _categoriesList.FirstOrDefault(x => x.Id == id);
        }

        public Category GetCategory(string title)
        {
            var obj = _categoriesList.FirstOrDefault(x => x.Title.Trim().ToLower() == title.Trim().ToLower());
            return obj;
        }

        public bool CategoryExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool CategoryExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
