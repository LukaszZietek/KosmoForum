using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;
using KosmoForumClient.Repo;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace KosmoForumClient.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly IForumPostRepository _forumRepo;
        private readonly ICategoryRepository _categoryRepo;
        public ForumPostController(IForumPostRepository forumRepo, ICategoryRepository categoryRepo)
        {
            _forumRepo = forumRepo;
            _categoryRepo = categoryRepo;

        }
        public IActionResult Index()
        {

            return View(new ForumPost(){});
        }

        public async Task<IActionResult> ForumPostInCategory(/*int id*/ string title) // category title
        {
            if (title == null)
            {
                return NotFound();
            }
           var categoryObj = await _categoryRepo.GetAsyncByTitle(SD.Categories, title);

            return View(new ForumPost()
            {
                Id = categoryObj.Id

            });
        }

        public async  Task<IActionResult> ReadForumPost(int id) // forumpost id
        {
            var obj = await _forumRepo.GetAsync(SD.ForumPosts, id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public async  Task<IActionResult> Upsert(int? id) // forumpost id
        {
            IEnumerable<Category> categoryList = await _categoryRepo.GetAllAsync(SD.Categories);

            ForumPostVM objVM = new ForumPostVM
            {
                CategoryList = categoryList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                }),
                forumPost = new ForumPost()
            };

            if (id == null)
            {
                return View(objVM);
            }
            objVM.forumPost = await _forumRepo.GetAsync(SD.ForumPosts, id.GetValueOrDefault());
            if (objVM.forumPost == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Upsert(ForumPostVM forumPostObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        byte[] p1 = null;
                        using (var fs1 = files[i].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }

                        forumPostObj.forumPost.Images.Add(new Image
                        {
                            Picture = p1
                        });
                    }
                }
                forumPostObj.forumPost.UserId = 1; // Zmienić tutaj userId
                //forumPostObj.forumPost.Date = DateTime.Now;
                if (forumPostObj.forumPost.Id == 0)
                {
                    await _forumRepo.CreateAsync(SD.ForumPosts, forumPostObj.forumPost);
                }
                else
                {
                    await _forumRepo.UpdateAsync(SD.ForumPosts, forumPostObj.forumPost.Id, forumPostObj.forumPost);
                }

                return RedirectToAction(nameof(Index));

            }
            else
            {
                IEnumerable<Category> categories = await _categoryRepo.GetAllAsync(SD.Categories);
                ForumPostVM forumVM = new ForumPostVM
                {
                    CategoryList = categories.Select(i => new SelectListItem
                    {
                        Text = i.Title,
                        Value = i.Id.ToString()
                    }),
                    forumPost = new ForumPost()
                };
                return View(forumVM);
            }
        }

        public async Task<IActionResult> GetAllForumPostsInCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var objToReturn = await _forumRepo.GetAllFromCategory(SD.ForumPosts, categoryId.GetValueOrDefault());
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
