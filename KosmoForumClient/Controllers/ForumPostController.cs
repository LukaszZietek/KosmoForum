using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KosmoForumClient.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly IForumPostRepository _forumRepo;
        public ForumPostController(IForumPostRepository forumRepo)
        {
            _forumRepo = forumRepo;

        }
        public IActionResult Index()
        {

            return View(new ForumPost(){});
        }

        public async  Task<IActionResult> Upsert(int? id)
        {
            ForumPost forumPost = new ForumPost();
            if (id == null)
            {
                return View(forumPost);
            }

            forumPost = await _forumRepo.GetAsync(SD.ForumPosts, id.GetValueOrDefault());
            if (forumPost == null)
            {
                return NotFound();
            }

            return View(forumPost);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Upsert(ForumPost forumPostObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
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

                        forumPostObj.Images.Add(new Image
                        {
                            AddTime = DateTime.Now,
                            ForumPostId = forumPostObj.Id,
                            UserId = 1, // Zmienić tutaj userId
                            Picture = p1
                        });
                    }
                }
                else
                {
                    var objFromDb = await _forumRepo.GetAsync(SD.ForumPosts, forumPostObj.Id);
                    foreach (var item in objFromDb.Images)
                    {
                        forumPostObj.Images.Add(item);
                    }
                }

                if (forumPostObj.Id == 0)
                {
                    await _forumRepo.CreateAsync(SD.ForumPosts, forumPostObj);
                }
                else
                {
                    await _forumRepo.UpdateAsync(SD.ForumPosts, forumPostObj.Id, forumPostObj);
                }

                return RedirectToAction(nameof(Index));

            }

            return View(forumPostObj);
        }

        public async Task<IActionResult> GetAllForumPostsInCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var objToReturn = _forumRepo.GetAllFromCategory(SD.ForumPosts, categoryId.GetValueOrDefault());
            return Json(new {data = objToReturn});

        }

        public async Task<IActionResult> GetAllForumPosts()
        {
            var forumPostObj = await _forumRepo.GetAllAsync(SD.ForumPosts);
            return Json(new {data = forumPostObj});
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _forumRepo.DeleteAsync(SD.ForumPosts, id);
            if (status == true)
            {
                return Json(new {success = true, message = "Usuwanie zakończyło się sukcesem!" });
            }

            return Json(new {success = false, message = "Usuwanie zakończone niepowodzeniem!"});
        }
    }
}
