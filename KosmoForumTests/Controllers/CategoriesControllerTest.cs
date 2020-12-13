using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using KosmoForum.Controllers;
using KosmoForum.Mapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KosmoForumTests.Controllers
{
    public class CategoriesControllerTest
    {
        private readonly Mock<ICategoryRepo> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoriesController _controller;

        public CategoriesControllerTest()
        {
            _mockRepo = new Mock<ICategoryRepo>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CategoriesController(_mockRepo.Object,_mockMapper.Object);
        }

        [Fact]
        public void GetCategories_WithEmptyDbShouldReturnNotFound()
        {
            var result = _controller.GetCategories();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetCategories_With2ElementShouldReturn2ElementAndOkResponse()
        {
            _mockRepo.Setup(repo => repo.GetAllCategories())
                .Returns(new List<Category>() { new Category() {CreationDateTime = DateTime.Now, Description = "Ala ma kota", Id = 0, Title = "Włosy"},
                    new Category() {CreationDateTime = DateTime.Now, Description = "Kot ma ale", Id = 1, Title = "Głowa"}});

            var result = _controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsType<List<CategoryDto>>(okResult.Value);
            Assert.Equal(2, categories.Count);
        }
    }
}
