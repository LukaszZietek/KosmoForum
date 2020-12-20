using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;

namespace KosmoForumTests.FakeRepo
{
    class CategoryFakeRepo
    {
        public List<Category> categoriesList;

        public CategoryFakeRepo()
        {
            categoriesList = new List<Category>()
            {
                new Category() {CreationDateTime = DateTime.Now, Description = "Ala ma kota", Id = 0, Title = "Włosy"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Kot ma ale", Id = 1, Title = "Głowa"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Transformers", Id = 2, Title = "Nogi"},
                new Category() {CreationDateTime = DateTime.Now, Description = "Czara ognia", Id = 3, Title = "Uszy"},
            };
        }

    }
}
