using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class OpinionsController : Controller
    {
        private readonly IOpinionRepository _opinionRepo;

        public OpinionsController(IOpinionRepository opinionRepo)
        {
            _opinionRepo = opinionRepo;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Upsert(int forumPostId, int? opinionId) // forumpostId and alternative u can call with opinion id to update it.
        {
            Opinion opinionObj = new Opinion();
            if (opinionId == null)
            {
                opinionObj.ForumPostId = forumPostId;
                return View(opinionObj);
            }

            opinionObj = await _opinionRepo.GetAsync(SD.Opinions, opinionId.GetValueOrDefault());
            if (opinionObj == null)
            {
                return NotFound();
            }

            opinionObj.ForumPostId = forumPostId;
            return View(opinionObj);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Upsert(Opinion opinionObj)
        {
            if (ModelState.IsValid)
            {
                if (opinionObj == null)
                {
                    return NotFound();
                }

                opinionObj.UserId = 1; // ZMIENIĆ TUTAJ USER ID

                if (opinionObj.Id == 0)
                {
                    await _opinionRepo.CreateAsync(SD.Opinions, opinionObj);
                }
                else
                {
                    await _opinionRepo.UpdateAsync(SD.Opinions, opinionObj.Id, opinionObj);
                }

                return RedirectToAction("ReadForumPost", "ForumPost", new {id = opinionObj.ForumPostId});
            }
            else
            {
                return View(opinionObj);
            }

        }


    }
}
