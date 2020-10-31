using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

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
            return View(new Category(){});
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            Category obj = new Category();
            if (id == null)
            {
                return View(obj);
            }

            var errorTupleobj = await _categoryRepo.GetAsync(SD.Categories, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (errorTupleobj.Item1 != "")
            {
                TempData["error"] = errorTupleobj.Item2;
                return View(obj);
            }

            return View(obj);



        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0) // Sprawdzenie czy istnieją jakieś pliki i wczytanie ich
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        p1 = Resizer.Resize(fs1, 50, 50);
                    }

                    obj.Image = p1;
                }
                else if (files.Count > 0 && obj.Id != 0)
                {
                    var objFromDb = await _categoryRepo.GetAsync(SD.Categories, obj.Id, HttpContext.Session.GetString("JWToken"));
                    if (objFromDb.Item1 != "")
                    {
                        TempData["error"] = objFromDb.Item1;
                        return View(obj);
                    }
                    obj.Image = objFromDb.Item2.Image;
                    
                }

                if (obj.Id == 0) // Oznacza to że nie istniał jeszcze w bazie danych
                {
                   var result = await _categoryRepo.CreateAsync(SD.Categories, obj, HttpContext.Session.GetString("JWToken"));
                   if (result.Item1 != "")
                   {
                       TempData["error"] = result.Item1;
                       return View(obj);
                   }
                }
                else
                {
                    await _categoryRepo.UpdateAsync(SD.Categories, obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }


        public async Task<IActionResult> GetAllCategories()
        {
            var dataTable = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));
            return Json(new {data = dataTable});
            //return Json(new {data = await _categoryRepo.GetAllAsync(SD.Categories)});

        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _categoryRepo.DeleteAsync(SD.Categories, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new {success = true, message = "Usuwanie zakończyło się sukcesem!"});
            }

            return Json(new {success = false, message = "Usuwanie zakończone niepowodzeniem!"});
        }
    }
}
