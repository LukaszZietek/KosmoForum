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
            if (categoryObj.Item1 != "")
            {
                return NotFound();
            }

            return View(new ForumPost()
            {
                Id = categoryObj.Item2.Id

            });
        }

        public async  Task<IActionResult> ReadForumPost(int id) // forumpost id
        {
            var obj = await _forumRepo.GetAsync(SD.ForumPosts, id, HttpContext.Session.GetString("JWToken"));

            if (obj.Item1 != "")
            {
                return NotFound();
            }

            var categoryObj = await _categoryRepo.GetAsync(SD.Categories, obj.Item2.CategoryId,
                HttpContext.Session.GetString("JWToken"));
            if (categoryObj.Item1 != "")
            {
                return NotFound();
            }

            ForumPostDetailsVM objVM = new ForumPostDetailsVM()
            {
                category = categoryObj.Item2,
                forumPost = obj.Item2
            };

            return View(objVM);
        }

        [Authorize]
        public async  Task<IActionResult> Upsert(int? id) // forumpost id
        {
            Tuple<string,IEnumerable<Category>> tupleResponse = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));
            if (tupleResponse.Item1 != "")
            {
                TempData["error"] = tupleResponse.Item1;
                return View(new ForumPostVM() {CategoryList = new List<SelectListItem>(), ForumPostModel = new ForumPost() {}});
            }
            IEnumerable<Category> categoryList = tupleResponse.Item2;

            ForumPostVM objVM = new ForumPostVM
            {
                CategoryList = categoryList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                }),
                ForumPostModel = new ForumPost()
            };

            if (id == null)
            {
                return View(objVM);
            }

            var forumPostTuple = await _forumRepo.GetAsync(SD.ForumPosts, id.GetValueOrDefault(),
                HttpContext.Session.GetString("JWToken"));

            if (forumPostTuple.Item1 != "")
            {
                TempData["error"] = forumPostTuple.Item1;
                return View(objVM);
            }

            objVM.ForumPostModel = forumPostTuple.Item2;

            return View(objVM);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upsert(ForumPostVM forumPostObj)
        {
            if (ModelState.IsValid)
            {
                var userTuple = await _accountRepo.GetUserId(SD.AccountApi, User.Identity.Name, HttpContext.Session.GetString("JWToken"));

                if (userTuple.Item1 != "")
                {
                    TempData["error"] = userTuple.Item1;
                    return View(forumPostObj);
                }

                ForumPost originalObj = null;
                if (forumPostObj.ForumPostModel.Id != 0)
                {
                    var errorTupleObj = await _forumRepo.GetAsync(SD.ForumPosts, forumPostObj.ForumPostModel.Id,
                        HttpContext.Session.GetString("JWToken"));
                    if (errorTupleObj.Item1 != "")
                    {
                        TempData["error"] = errorTupleObj.Item1;
                        return View(forumPostObj);
                    }

                    originalObj = errorTupleObj.Item2;
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
                            p1 = Resizer.Resize(fs1, 200, 200);
                        }

                        forumPostObj.ForumPostModel.Images.Add(new Image
                        {
                            Picture = p1,
                            UserId = userTuple.Item2
                        });
                        counter++;
                    }
                }

                forumPostObj.ForumPostModel.UserId = userTuple.Item2;

                if (forumPostObj.ForumPostModel.Id == 0)
                {
                    var result = await _forumRepo.CreateAsync(SD.ForumPosts, forumPostObj.ForumPostModel,HttpContext.Session.GetString("JWToken"));
                    if (result.Item1 != "")
                    {
                        TempData["error"] = result.Item1;
                        return View(forumPostObj);
                    }
                }
                else
                {
                    var result = await _forumRepo.UpdateAsync(SD.ForumPosts, forumPostObj.ForumPostModel.Id, forumPostObj.ForumPostModel, HttpContext.Session.GetString("JWToken"));
                    if (result.Item1 != "")
                    {
                        TempData["error"] = result.Item1;
                        return View(forumPostObj);
                    }
                }

                return RedirectToAction(nameof(Index));

            }
            else
            {
                var tupleResponse = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));
                if (tupleResponse.Item1 != "")
                {
                    TempData["error"] = tupleResponse.Item1;
                    return View(new ForumPostVM() { CategoryList = new List<SelectListItem>(), ForumPostModel = new ForumPost() { } });
                }
                ForumPostVM forumVM = new ForumPostVM
                {
                    CategoryList = tupleResponse.Item2.Select(i => new SelectListItem
                    {
                        Text = i.Title,
                        Value = i.Id.ToString()
                    }),
                    ForumPostModel = new ForumPost()
                };
                return View(new ForumPostVM() { CategoryList = new List<SelectListItem>(),ForumPostModel = new ForumPost()});
            }
        }

        public async Task<IActionResult> GetAllForumPostsInCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var objToReturn = await _forumRepo.GetAllFromCategory(SD.ForumPosts, categoryId.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return Json(new {data = objToReturn.Item2});

        }

        public async Task<IActionResult> GetAllForumPosts()
        {
            var forumPostObj = await _forumRepo.GetAllAsync(SD.ForumPosts, HttpContext.Session.GetString("JWToken"));
            return Json(new {data = forumPostObj.Item2});
        }


        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _forumRepo.DeleteAsync(SD.ForumPosts, id, HttpContext.Session.GetString("JWToken"));
            if (status.Item2)
            {
                return Json(new {success = true, message = "Usuwanie zakończyło się sukcesem!" });
            }

            return Json(new {success = false, message = "Usuwanie zakończone niepowodzeniem!"});
        }
    }
}
