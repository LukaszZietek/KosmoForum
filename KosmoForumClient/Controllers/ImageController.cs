using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Repo.IRepo;
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

        public async Task<IActionResult> DeleteImage(int id)
        {
            var status = await _imgRepo.DeleteAsync(SD.Images, id);
            if (status)
            {
                return Json(new { success = true, message = "Usuwanie zakończyło się sukcesem!" });
            }
            return Json(new { success = false, message = "Usuwanie zakończone niepowodzeniem!" });
        }
    }
}
