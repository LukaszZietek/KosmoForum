using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace KosmoForum.Controllers
{
    [Route("api/v{version:apiVersion}/Opinions")]
    [ApiController]
    [ProducesResponseType(400)]
    public class OpinionsController : ControllerBase
    {
        private readonly IOpinionRepo _repo;
        private readonly IMapper _mapper;

        public OpinionsController(IOpinionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Opinion by Id
        /// </summary>
        /// <param name="id">Opinion Id</param>
        /// <returns></returns>

        [HttpGet("{id:int}",Name ="GetOpinion")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(OpinionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]

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
        /// <summary>
        /// Get all opinions
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetOpinions")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(OpinionDto))]
        [ProducesDefaultResponseType]
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

        /// <summary>
        /// Get all opinion in specific forum post
        /// </summary>
        /// <param name="forumPostId">Forum post's id</param>
        /// <returns></returns>

        [HttpGet("[action]/{forumPostId:int}", Name = "GetOpinionsInForumPost")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200,Type = typeof(OpinionDto))]
        [ProducesDefaultResponseType]
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
        /// <summary>
        /// Create opinion
        /// </summary>
        /// <param name="opinionCreateObj">Opinion object</param>
        /// <returns></returns>
        [HttpPost(Name = "CreateOpinion")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Opinion))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
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

            return CreatedAtRoute("GetOpinion", new {Version = HttpContext.GetRequestedApiVersion().ToString() ,id = opinion.Id}, opinion);
        }

        /// <summary>
        /// Update already existing opinion
        /// </summary>
        /// <param name="id">Opinion Id</param>
        /// <param name="opinionUpdateObj">Opinion Object</param>
        /// <returns></returns>
        [HttpPatch("{id:int}",Name = "UpdateOpinion")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Authorize]
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

        /// <summary>
        /// Delete already existing opinion
        /// </summary>
        /// <param name="id">Opinion Id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}",Name = "DeleteOpinion")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [Authorize]
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
