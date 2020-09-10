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
using Microsoft.AspNetCore.Mvc.Formatters;

namespace KosmoForum.Controllers
{
    [Route("api/Opinions")]
    [ApiController]
    public class OpinionsController : ControllerBase
    {
        private readonly IOpinionRepo _repo;
        private readonly IMapper _mapper;

        public OpinionsController(IOpinionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id:int}",Name ="GetOpinion")]
        public IActionResult GetOpinion(int id)
        {
            var obj = _repo.GetOpinion(id);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<OpinionDto>(obj);
            return Ok(objDto);
        }

        [HttpGet]
        public IActionResult GetOpinions()
        {
            var listObj = _repo.GetAllOpinions();
            if (listObj == null)
            {
                return NotFound();
            }

            var opinionDtos = new List<OpinionDto>();
            foreach (var item in listObj)
            {
                opinionDtos.Add(_mapper.Map<OpinionDto>(item));
            }

            return Ok(opinionDtos);
        }

        [HttpGet("[action]/{forumPostId:int}")]
        public IActionResult GetOpinionsInForumPost(int forumPostId)
        {
            var obj = _repo.GetAllOpinionsInPost(forumPostId);
            if (obj == null)
            {
                return NotFound();
            }
            var opinionDtos = new List<OpinionDto>();
            foreach (var item in obj)
            {
                opinionDtos.Add(_mapper.Map<OpinionDto>(item));
            }

            return Ok(opinionDtos);
        }

        [HttpPost]
        public IActionResult CreateOpinion([FromBody] OpinionCreateDto opinionCreateObj)
        {
            if (opinionCreateObj == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var opinion = _mapper.Map<Opinion>(opinionCreateObj);
            opinion.CreationDateTime = DateTime.Now;
            if (!_repo.CreateOpinion(opinion))
            {
                ModelState.AddModelError("",$"Error occurred during creating opinion with content : {opinion.Content}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetOpinion", new {id = opinion.Id}, opinion);
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateOpinion(int id,[FromBody] OpinionUpdateDto opinionUpdateObj)
        {
            if (opinionUpdateObj == null || id != opinionUpdateObj.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var opinionObj = _mapper.Map<Opinion>(opinionUpdateObj);
            opinionObj.CreationDateTime = DateTime.Now;
            if (!_repo.UpdateOpinion(opinionObj))
            {
                ModelState.AddModelError("", $"Error occurred during updating opinion with this content: {opinionObj.Content}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOpinion(int id)
        {
            if (!_repo.OpinionIfExist(id))
            {
                return NotFound();
            }

            var opinion = _repo.GetOpinion(id);
            if (!_repo.DeleteOpinion(opinion))
            {
                ModelState.AddModelError("",$"Error occurred during deleting opinion with content which you see below: {opinion.Content}");
                return StatusCode(500, ModelState);
            }

            return NoContent();



        }

    }
}
