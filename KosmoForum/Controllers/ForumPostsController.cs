using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{
    [Route("api/ForumPosts")]
    [ApiController]
    public class ForumPostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IForumPostRepo _repo;

        public ForumPostsController(IMapper mapper, IForumPostRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetForumPosts()
        {
            var objList = _repo.GetAllPosts();
            var forumPostsDto = new List<ForumPostDto>();
            
            if (objList == null)
            {
                return NotFound();
            }

            foreach (var item in objList)
            {
                forumPostsDto.Add(_mapper.Map<ForumPostDto>(item));
            }

            return Ok(forumPostsDto);

        }

        [HttpGet("{id}", Name = "GetForumPost")]
        public IActionResult GetForumPost(int id)
        {
            var obj = _repo.GetPost(id);
            if (obj == null)
            {
                return NotFound();
            }

            var forumPostDto = _mapper.Map<ForumPostDto>(obj);
            return Ok(forumPostDto);
        }

        [HttpGet("[action]/{categoryId:int}")]
        public IActionResult GetForumPostsInCategory(int categoryId)
        {
            var forumPosts = _repo.GetAllForumPostsInCategory(categoryId);
            if (forumPosts == null)
            {
                return NotFound();
            }

            var forumPostDtos = new List<ForumPostDto>();
            foreach (var item in forumPosts)
            {
                forumPostDtos.Add(_mapper.Map<ForumPostDto>(item));
            }

            return Ok(forumPostDtos);
        }


        [HttpPost]
        public IActionResult CreateForumPost([FromBody] ForumPostCreateDto forumPostDto)
        {
            if (forumPostDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.ForumPostIfExist(forumPostDto.Title))
            {
                ModelState.AddModelError("","Post with this title already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forumPost = _mapper.Map<ForumPost>(forumPostDto);
            forumPost.Date = DateTime.Now;

            if (!_repo.CreateForumPost(forumPost))
            {
                ModelState.AddModelError("",$"Error occurred during saving object with title: {forumPost.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetForumPost", new {id = forumPost.Id}, forumPost);

        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateForumPost(int id, [FromBody] ForumPostUpdateDto forumPostDto)
        {
            if (forumPostDto == null || id != forumPostDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obj = _repo.GetPost(forumPostDto.Title);
            if (obj != null) // Sprawdzanie czy tytuł po zmianie dalej będzie unikalny
            {
                if (obj.Id != forumPostDto.Id)
                {
                    ModelState.AddModelError("", "Category with this title already exists");
                    return BadRequest(ModelState);
                }
            }


            var forumPost = _mapper.Map<ForumPost>(forumPostDto);
            forumPost.Date = DateTime.Now;

            if (!_repo.UpdateForumPost(forumPost))
            {
                ModelState.AddModelError("", $"Error occurred during updating post with title: {forumPost.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteForumPost(int id)
        {
            if (!_repo.ForumPostIfExist(id))
            {
                return BadRequest(ModelState);
            }

            var forumObj = _repo.GetPost(id);
            if (!_repo.DeleteForumPost(forumObj))
            {
                ModelState.AddModelError("",$"Error occurred during deleting object with title: {forumObj.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }


    }
}
