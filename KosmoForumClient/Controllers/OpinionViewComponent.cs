using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class OpinionViewComponent : ViewComponent
    {
        private readonly IOpinionRepository _opinionRepo;
        private readonly IAccountRepository _accountRepo;

        public OpinionViewComponent(IOpinionRepository opinionRepo, IAccountRepository accountRepo)
        {
            _opinionRepo = opinionRepo;
            _accountRepo = accountRepo;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync(int forumPostId, int? opinionId)
        {
            Opinion opinionObj = new Opinion();
            if (opinionId == null)
            {
                opinionObj.ForumPostId = forumPostId;
                return View("Default",opinionObj);
            }

            var opinionObjTuple = await _opinionRepo.GetAsync(SD.Opinions, opinionId.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (opinionObjTuple.Item1 != "")
            {
                TempData["error"] = opinionObjTuple.Item1;
                return View("Default", opinionObj);
            }

            opinionObj = opinionObjTuple.Item2;

            opinionObj.ForumPostId = forumPostId;
            return View("Default", opinionObj);

        }
    }
}
