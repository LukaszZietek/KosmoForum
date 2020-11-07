using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageRepository _imgRepo;

        public ImageController(IImageRepository imgRepo)
        {
            _imgRepo = imgRepo;
        }

        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var status = await _imgRepo.DeleteAsync(SD.Images, id, HttpContext.Session.GetString("JWToken"));
            if (status.Item2)
            {
                return Json(new { success = true, message = "Usuwanie zakończyło się sukcesem!" });
            }
            return Json(new { success = false, message = "Usuwanie zakończone niepowodzeniem!" });
        }
    }
}
