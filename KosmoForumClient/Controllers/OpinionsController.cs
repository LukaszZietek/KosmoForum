using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class OpinionsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Upsert(int? id)
        {

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Upsert(Opinion opinionObj)
        {

        }
    }
}
