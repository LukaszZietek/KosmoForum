using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/Image")]
    [ProducesResponseType(400)]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepo _imgRepo;

        public ImageController(IImageRepo imgRepo)
        {
            _imgRepo = imgRepo;
        }

        /// <summary>
        /// Delete image with specific Id
        /// </summary>
        /// <param name="id">Image Id</param>
        /// <returns></returns>
         [HttpDelete("{id}", Name = "DeleteImage")]
         [ProducesResponseType(204)]
         [ProducesResponseType(500)]
         [ProducesResponseType(404)] 
         [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteImage(int id)
        {
            if (!_imgRepo.ifExists(id))
            {
                return NotFound();
            }

            if (!_imgRepo.DeleteImage(id))
            {
                ModelState.AddModelError("", $"Error occurred during deleting image");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
