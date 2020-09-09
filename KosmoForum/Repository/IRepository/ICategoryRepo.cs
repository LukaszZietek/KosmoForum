using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;

namespace KosmoForum.Repository.IRepository
{
    public interface ICategoryRepo
    {
        ICollection<Category> GetAllCategories();

        Category GetCategory(int id);
        Category GetCategory(string title);

        bool CategoryExists(int id);
        bool CategoryExists(string name);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();



    }
}
