using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
