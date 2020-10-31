using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [ApiController]
    [Route("api/v{version:apiVersion}/Categories")]
    [ProducesResponseType(400)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Get list of available categories
        /// </summary>
        /// <returns></returns>

        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(200,Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategories()
        {
            var categories = _repo.GetAllCategories();
            if (categories == null)
            {
                return NotFound(new {message = "Database doesn't contain any categories"});
            }
            List<CategoryDto> objDtos = new List<CategoryDto>();

            foreach (var item in categories)
            {
                objDtos.Add(_mapper.Map<CategoryDto>(item));
            }

            return Ok(objDtos);
        }

        /// <summary>
        /// Get category with specific id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(CategoryDto))]
        [ProducesDefaultResponseType]
        [HttpGet("{id:int}", Name = "GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var obj = _repo.GetCategory(id);
            if (obj == null)
            {
                return NotFound(new {message = "Category with this id isn't exist in database"});
            }

            var objDto = _mapper.Map<CategoryDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Get category with specific title
        /// </summary>
        /// <param name="title">category title</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesDefaultResponseType]
        [HttpGet("[action]/{title}", Name = "GetCategoryByTitle")]
        public IActionResult GetCategoryByTitle(string title)
        {
            if (title == null)
            {
                return BadRequest(new {message = "Title is null"});
            }

            var obj = _repo.GetCategory(title);
            if (obj == null)
            {
                return NotFound(new {message = "Object with this title isn't exist in database"});
            }

            var objMap = _mapper.Map<CategoryDto>(obj);

            return Ok(objMap);

        }

        /// <summary>
        /// Create a category
        /// </summary>
        /// <param name="categoryDto">Category object which should be create</param>
        /// <returns></returns>

        [HttpPost(Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "admin")]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});

                //return BadRequest(ModelState);
            }

            if (_repo.CategoryExists(categoryDto.Title))
            {
                //ModelState.AddModelError("","Category with this title already exists");
                //return StatusCode(404, ModelState);
                return StatusCode(404, new { message = "Category with this title already exists" });
            }

            if (!ModelState.IsValid)
            {
              
                return BadRequest(new { message = ModelStateToString.ConvertModelStateToString(ModelState) });

                //return BadRequest(ModelState);
            }

            var categoryObj = _mapper.Map<Category>(categoryDto);
            categoryObj.CreationDateTime = DateTime.Now;
            if (!_repo.CreateCategory(categoryObj))
            {
                //ModelState.AddModelError("", $"Something went wrong when saving the record {categoryDto.Title}");
                return StatusCode(500, new {message = $"Something went wrong when saving the record {categoryDto.Title}" });
            }

            return CreatedAtRoute("GetCategory", new {Version = HttpContext.GetRequestedApiVersion().ToString() ,id = categoryObj.Id}, categoryObj);

        }

        /// <summary>
        /// Update the previous created category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <param name="categoryUpdateDto">Category Object</param>
        /// <returns></returns>

        [HttpPatch("{id}",Name = "UpdateCategory")]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null || categoryUpdateDto.Id != id)
            {
                return BadRequest(new { message = ModelStateToString.ConvertModelStateToString(ModelState) });

                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelStateToString.ConvertModelStateToString(ModelState) });
                //return BadRequest(ModelState);
            }


            var obj = _repo.GetCategory(categoryUpdateDto.Title);
            if (obj != null) // Sprawdzanie czy tytuł po zmianie dalej będzie unikalny
            {
                if (obj.Id != categoryUpdateDto.Id)
                {
                    //ModelState.AddModelError("", "Category with this title already exists");
                    return BadRequest(new {message = "Category with this title already exists" });
                }
            }

            var categoryObj = _mapper.Map<Category>(categoryUpdateDto);

            if (!_repo.UpdateCategory(categoryObj))
            {
                //ModelState.AddModelError("", $"Error occured during updating object: {categoryUpdateDto.Title}");
                return StatusCode(500, new {message = $"Error occured during updating object: {categoryUpdateDto.Title}"});
            }

            return NoContent();


        }

        /// <summary>
        /// Delete previous created category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>

        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteCategory(int id)
        {
            if (!_repo.CategoryExists(id))
            {
                return NotFound(new {message = ModelStateToString.ConvertModelStateToString(ModelState)});
            }

            var categoryObj = _repo.GetCategory(id);

            if (!_repo.DeleteCategory(categoryObj))
            {
                //ModelState.AddModelError("", $"Error occurred during deleting object : {categoryObj.Title}");
                return StatusCode(500, new {message = $"Error occurred during deleting object : {categoryObj.Title}"});
            }

            return NoContent();

        }


    }
}
