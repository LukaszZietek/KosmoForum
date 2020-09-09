using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForum.Controllers
{
    [ApiController]
    [Route("api/Categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _repo.GetAllCategories();
            if (categories == null)
            {
                return NotFound();
            }
            List<CategoryDto> objDtos = new List<CategoryDto>();

            foreach (var item in categories)
            {
                objDtos.Add(_mapper.Map<CategoryDto>(item));
            }

            return Ok(objDtos);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var obj = _repo.GetCategory(id);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<CategoryDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.CategoryExists(categoryDto.Title))
            {
                ModelState.AddModelError("","Category with this title already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryObj = _mapper.Map<Category>(categoryDto);
            categoryObj.CreationDateTime = DateTime.Now;
            if (!_repo.CreateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {categoryDto.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new {id = categoryObj.Id}, categoryObj);

        }

        [HttpPatch("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null || categoryUpdateDto.Id != id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var obj = _repo.GetCategory(categoryUpdateDto.Title);
            if (obj != null) // Sprawdzanie czy tytuł po zmianie dalej będzie unikalny
            {
                if (obj.Id != categoryUpdateDto.Id)
                {
                    ModelState.AddModelError("", "Category with this title already exists");
                    return BadRequest(ModelState);
                }
            }

            var categoryObj = _mapper.Map<Category>(categoryUpdateDto);

            if (!_repo.UpdateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Error occured during updating object: {categoryUpdateDto.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            if (!_repo.CategoryExists(id))
            {
                return NotFound(ModelState);
            }

            var categoryObj = _repo.GetCategory(id);

            if (!_repo.DeleteCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Error occurred during deleting object : {categoryObj.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
