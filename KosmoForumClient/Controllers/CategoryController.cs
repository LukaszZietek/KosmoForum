using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Http;
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
            return View(new Category(){});
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category obj = new Category();
            if (id == null)
            {
                return View(obj);
            }

            obj = await _categoryRepo.GetAsync(SD.Categories, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);



        }

        [ValidateAntiForgeryToken]
        [HttpPost]
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
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    obj.Image = p1;
                }
                else
                {
                    var objFromDb = await _categoryRepo.GetAsync(SD.Categories, obj.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Image = objFromDb.Image;
                }

                if (obj.Id == 0) // Oznacza to że nie istniał jeszcze w bazie danych
                {
                    await _categoryRepo.CreateAsync(SD.Categories, obj, HttpContext.Session.GetString("JWToken"));
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
