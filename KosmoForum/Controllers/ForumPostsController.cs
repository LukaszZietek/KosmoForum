using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{
    [Route("api/v{version:apiVersion}/ForumPosts")]
    [ApiController]
    [ProducesResponseType(400)]
    public class ForumPostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IForumPostRepo _repo;
        private readonly IUserRepo _userRepo;

        public ForumPostsController(IMapper mapper, IForumPostRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Get all forum posts
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<ForumPostDto>))]
        [HttpGet(Name = "GetForumPosts")]
        public IActionResult GetForumPosts()
        {
            var objList = _repo.GetAllPosts();
            var forumPostsDto = new List<ForumPostDto>();
            
            if (objList == null)
            {
                return NotFound(new {message = "Database doesn't contain any forum posts"});
            }

            foreach (var item in objList)
            {
                forumPostsDto.Add(_mapper.Map<ForumPostDto>(item));
            }

            return Ok(forumPostsDto);

        }

        /// <summary>
        /// Get forum post with specific id
        /// </summary>
        /// <param name="id">forum post's id</param>
        /// <returns></returns>

        [HttpGet("{id}", Name = "GetForumPost")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200,Type =typeof(ForumPostDto))]
        [ProducesDefaultResponseType]
        public IActionResult GetForumPost(int id)
        {
            var obj = _repo.GetPost(id);
            if (obj == null)
            {
                return NotFound(new {message = "Forum post with this specific id doesn't exist in database"});
            }

            var forumPostDto = _mapper.Map<ForumPostDto>(obj);
            return Ok(forumPostDto);
        }


        /// <summary>
        /// Get all forum posts in specific category
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <returns></returns>
        [HttpGet("[action]/{categoryId:int}",Name = "GetForumPostsInCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200,Type = typeof(ForumPostDto))]
        [ProducesDefaultResponseType]

        public IActionResult GetForumPostsInCategory(int categoryId)
        {
            var forumPosts = _repo.GetAllForumPostsInCategory(categoryId);
            if (forumPosts == null)
            {
                return NotFound(new {message = "This category doesn't contain forum posts "});
            }

            var forumPostDtos = new List<ForumPostDto>();
            foreach (var item in forumPosts)
            {
                forumPostDtos.Add(_mapper.Map<ForumPostDto>(item));
            }

            return Ok(forumPostDtos);
        }

        /// <summary>
        /// Get all forum posts which belong to current user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(ForumPostDto))]
        [ProducesDefaultResponseType]
        [HttpGet("[action]")]
        public IActionResult GetForumPostsForUser()
        {
            int currentUserId = 0;
            try
            {
                currentUserId = Int32.Parse(User.Identity.Name);
            }
            catch (FormatException exception)
            {
                currentUserId = 0;
            }
            if (currentUserId == 0)
            {
                ModelState.AddModelError("", "To access this action you have to login");
                return BadRequest(ModelState);
            }

            var forumPosts = _repo.GetAllForumPostsForUser(currentUserId);
            if (forumPosts == null)
            {
                return NotFound( new { message = "This user doesn't have any forum post"});
            }

            var forumPostDtos = new List<ForumPostDto>();
            foreach (var item in forumPosts)
            {
                forumPostDtos.Add(_mapper.Map<ForumPostDto>(item));
            }

            return Ok(forumPostDtos);
        }

        /// <summary>
        /// Create forum post
        /// </summary>
        /// <param name="forumPostDto">Forum post object</param>
        /// <returns></returns>
        [HttpPost(Name = "CreateForumPost")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public IActionResult CreateForumPost([FromBody] ForumPostCreateDto forumPostDto)
        {
            if (forumPostDto == null)
            {
                //return BadRequest(ModelState);
                return BadRequest(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});
            }

            if (_repo.ForumPostIfExist(forumPostDto.Title))
            {
                //ModelState.AddModelError("","Post with this title already exists");
                return StatusCode(404,new {message = "Post with this title already exists" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});
                //return BadRequest(ModelState);
            }

            var forumPost = _mapper.Map<ForumPost>(forumPostDto);
            forumPost.Date = DateTime.Now;

            if (!_repo.CreateForumPost(forumPost))
            {
                //ModelState.AddModelError("",$"Error occurred during saving object with title: {forumPost.Title}");
                return StatusCode(500, new {message = $"Error occurred during saving object with title: {forumPost.Title}" });
            }

            return CreatedAtRoute("GetForumPost", new {Version = HttpContext.GetRequestedApiVersion().ToString(), id = forumPost.Id}, forumPost);

        }
        /// <summary>
        /// Update already existing forum post
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="forumPostDto">Post Object</param>
        /// <returns></returns>
        [HttpPatch("{id:int}",Name = "UpdateForumPost")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public IActionResult UpdateForumPost(int id, [FromBody] ForumPostUpdateDto forumPostDto)
        {
            if (forumPostDto == null || id != forumPostDto.Id)
            {
                /*return BadRequest(ModelState);*/
                return BadRequest(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});
            }

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return BadRequest(new { message = ModelStateToString.ConvertModelStateToString(ModelState)});
            }

            var obj = _repo.GetPost(forumPostDto.Title);
            if (obj != null) // Sprawdzanie czy tytuł po zmianie dalej będzie unikalny
            {
                if (obj.Id != forumPostDto.Id)
                {
                    //ModelState.AddModelError("", "Category with this title already exists");
                    return BadRequest(new {message = "Category with this title already exists" });
                }
            }


            var forumPost = _mapper.Map<ForumPost>(forumPostDto);
            forumPost.Date = DateTime.Now;

            if (!_repo.UpdateForumPost(forumPost))
            {
                //ModelState.AddModelError("", $"Error occurred during updating post with title: {forumPost.Title}");
                return StatusCode(500, new {message = $"Error occurred during updating post with title: {forumPost.Title}" });
            }

            return NoContent();

        }

        /// <summary>
        /// Delete already existing forum post
        /// </summary>
        /// <param name="id">Forum post id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteForumPost")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public IActionResult DeleteForumPost(int id)
        {
            if (!_repo.ForumPostIfExist(id))
            {
                //return BadRequest(ModelState);
                return BadRequest(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});
            }

            var forumObj = _repo.GetPost(id);
            if (!_repo.DeleteForumPost(forumObj))
            {
                //ModelState.AddModelError("",$"Error occurred during deleting object with title: {forumObj.Title}");
                return StatusCode(500, new {message = $"Error occurred during deleting object with title: {forumObj.Title}" });
            }

            return NoContent();


        }


    }
}
