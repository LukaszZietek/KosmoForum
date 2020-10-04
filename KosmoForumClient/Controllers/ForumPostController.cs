using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;
using KosmoForumClient.Repo;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccountRepository _accountRepo;
        public ForumPostController(IForumPostRepository forumRepo, ICategoryRepository categoryRepo, IAccountRepository accountRepo)
        {
            _forumRepo = forumRepo;
            _categoryRepo = categoryRepo;
            _accountRepo = accountRepo;

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
           var categoryObj = await _categoryRepo.GetAsyncByTitle(SD.Categories, title, HttpContext.Session.GetString("JWToken"));

            return View(new ForumPost()
            {
                Id = categoryObj.Id

            });
        }

        public async  Task<IActionResult> ReadForumPost(int id) // forumpost id
        {
            var obj = await _forumRepo.GetAsync(SD.ForumPosts, id, HttpContext.Session.GetString("JWToken"));

            if (obj == null)
            {
                return NotFound();
            }

            ForumPostDetailsVM objVM = new ForumPostDetailsVM()
            {
                category = await _categoryRepo.GetAsync(SD.Categories, obj.CategoryId, HttpContext.Session.GetString("JWToken")),
                forumPost = obj
            };

            return View(objVM);
        }

        [Authorize]
        public async  Task<IActionResult> Upsert(int? id) // forumpost id
        {
            IEnumerable<Category> categoryList = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));

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
            objVM.forumPost = await _forumRepo.GetAsync(SD.ForumPosts, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.forumPost == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upsert(ForumPostVM forumPostObj)
        {
            if (ModelState.IsValid)
            {
                var userId = await _accountRepo.GetUserId(SD.AccountApi, User.Identity.Name, HttpContext.Session.GetString("JWToken"));

                ForumPost originalObj = null;
                if (forumPostObj.forumPost.Id != 0)
                {
                   originalObj = await _forumRepo.GetAsync(SD.ForumPosts, forumPostObj.forumPost.Id,
                        HttpContext.Session.GetString("JWToken"));
                }

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    int counter = 0;
                    if (originalObj != null)
                    {
                        counter = originalObj.Images.Count;
                    }
                    for (int i = 0; i < files.Count && counter < 3; i++)
                    {
                        byte[] p1 = null;
                        using (var fs1 = files[i].OpenReadStream())
                        {
                            //using (var ms1 = new MemoryStream())
                            //{
                            //    fs1.CopyTo(ms1);
                            //    p1 = ms1.ToArray();
                            //}
                            p1 = Resizer.Resize(fs1, 200, 200);
                        }

                        forumPostObj.forumPost.Images.Add(new Image
                        {
                            Picture = p1,
                            UserId = userId
                        });
                        counter++;
                    }
                }

                forumPostObj.forumPost.UserId = userId;

                if (forumPostObj.forumPost.Id == 0)
                {
                    await _forumRepo.CreateAsync(SD.ForumPosts, forumPostObj.forumPost,HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _forumRepo.UpdateAsync(SD.ForumPosts, forumPostObj.forumPost.Id, forumPostObj.forumPost, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));

            }
            else
            {
                IEnumerable<Category> categories = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));
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

            var objToReturn = await _forumRepo.GetAllFromCategory(SD.ForumPosts, categoryId.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return Json(new {data = objToReturn});

        }

        public async Task<IActionResult> GetAllForumPosts()
        {
            var forumPostObj = await _forumRepo.GetAllAsync(SD.ForumPosts, HttpContext.Session.GetString("JWToken"));
            return Json(new {data = forumPostObj});
        }


        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _forumRepo.DeleteAsync(SD.ForumPosts, id, HttpContext.Session.GetString("JWToken"));
            if (status == true)
            {
                return Json(new {success = true, message = "Usuwanie zakończyło się sukcesem!" });
            }

            return Json(new {success = false, message = "Usuwanie zakończone niepowodzeniem!"});
        }
    }
}
