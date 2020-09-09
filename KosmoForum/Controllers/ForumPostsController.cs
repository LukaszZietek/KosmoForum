using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        [HttpGet("{id}")]
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


    }
}
