using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KosmoForumClient.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IForumPostRepository _forumRepo;
        private readonly IAccountRepository _accountRepo;

        public UserController(IForumPostRepository forumRepo, IAccountRepository accountRepo)
        {
            _forumRepo = forumRepo;
            _accountRepo = accountRepo;
        }

        public IActionResult MyForumPosts()
        {
            //var forumPosts = _forumRepo.GetAllBelongsToUser(SD.ForumPosts, HttpContext.Session.GetString("JWToken"));
            //if (forumPosts == null)
            //{
            //    return NotFound();
            //}

            return View(new ForumPost(){});
        }

        public async Task<IActionResult> ChangeAvatar()
        {
            Tuple<string, byte[]> userAvatarTuple =
                await _accountRepo.GetUserAvatar(SD.AccountApi, HttpContext.Session.GetString("JWToken"));

            if (userAvatarTuple.Item1 != "")
            {
                TempData["error"] = userAvatarTuple.Item1;
                return View(new User() {Avatar = new byte[0], Username = User.Identity.Name});
            }

            User currentUser = new User()
            {
                Username = User.Identity.Name,
                Avatar = userAvatarTuple.Item2
            };

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAvatar(User userObj)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null;
                using (var fs1 = files[0].OpenReadStream())
                {
                    p1 = Resizer.Resize(fs1, 50, 50);
                }

                userObj.Avatar = p1;
            }
            Tuple<string,bool> response =  await _accountRepo.UpdateUserAvatar(SD.AccountApi, userObj.Avatar, HttpContext.Session.GetString("JWToken"));
            if (response.Item1 != "")
            {
                TempData["error"] = response.Item1;
                return View(userObj);
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordVM() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM passwordModel)
        {
            if (ModelState.IsValid)
            {

                Tuple<string,bool> requestState = await _accountRepo.ChangePassword(SD.AccountApi, passwordModel,
                    HttpContext.Session.GetString("JWToken"));

                if (requestState.Item1 != "")
                {
                    TempData["error"] = requestState.Item1;
                    return View(passwordModel);
                }


                return RedirectToAction("Index", "Home");
            }

            return View(passwordModel);
        }

        public async Task<IActionResult> GetMyForumPosts()
        {
            var forumPosts = await _forumRepo.GetAllBelongsToUser(SD.ForumPosts, HttpContext.Session.GetString("JWToken"));
            return Json(new {data = forumPosts});

        }
    }
}
