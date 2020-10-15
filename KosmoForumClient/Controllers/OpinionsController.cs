﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class OpinionsController : Controller
    {
        private readonly IOpinionRepository _opinionRepo;
        private readonly IAccountRepository _accountRepo;

        public OpinionsController(IOpinionRepository opinionRepo, IAccountRepository accountRepo)
        {
            _opinionRepo = opinionRepo;
            _accountRepo = accountRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(new Opinion(){});
        }

        [Authorize]
        public async Task<IActionResult> Upsert(int forumPostId, int? opinionId) // forumpostId and alternative u can call with opinion id to update it.
        {
            Opinion opinionObj = new Opinion();
            if (opinionId == null)
            {
                opinionObj.ForumPostId = forumPostId;
                return View(opinionObj);
            }

            opinionObj = await _opinionRepo.GetAsync(SD.Opinions, opinionId.GetValueOrDefault(),HttpContext.Session.GetString("JWToken"));
            if (opinionObj == null)
            {
                return NotFound();
            }

            opinionObj.ForumPostId = forumPostId;
            return View(opinionObj);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upsert(Opinion opinionObj)
        {
            if (ModelState.IsValid)
            {
                if (opinionObj == null)
                {
                    return NotFound();
                }

                opinionObj.UserId = await _accountRepo.GetUserId(SD.AccountApi,User.Identity.Name, HttpContext.Session.GetString("JWToken"));

                if (opinionObj.Id == 0)
                {
                    await _opinionRepo.CreateAsync(SD.Opinions, opinionObj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _opinionRepo.UpdateAsync(SD.Opinions, opinionObj.Id, opinionObj, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction("ReadForumPost", "ForumPost", new {id = opinionObj.ForumPostId});
            }
            else
            {
                return View(opinionObj);
            }

        }


        [Authorize]
        public async Task<IActionResult> GetAllOpinionForUser()
        {
            var obj = await _opinionRepo.GetUserOpinion(SD.Opinions, HttpContext.Session.GetString("JWToken"));


            return Json(new {data = obj});
        }


    }
}
