using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using KosmoForum.Controllers;
using KosmoForum.Mapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository.IRepository;
using KosmoForumTests.FakeRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace KosmoForumTests.Controllers
{
    public class CategoriesControllerTest
    {
        private readonly Mock<ICategoryRepo> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoriesController _controller;
        private readonly CategoryFakeRepo _dummyData;

        public CategoriesControllerTest()
        {
            _mockRepo = new Mock<ICategoryRepo>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CategoriesController(_mockRepo.Object,_mockMapper.Object);
            _dummyData = new CategoryFakeRepo();
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
                .Returns(_dummyData.categoriesList);
            foreach (var item in _dummyData.categoriesList)
            {
                _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(item))
                    .Returns(new CategoryDto()
                        {
                            Description = item.Description, 
                            Id = item.Id,
                            Image = item.Image,
                            Title = item.Title
                        });
            }

            var result = _controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsType<List<CategoryDto>>(okResult.Value);
            Assert.Equal(_dummyData.categoriesList.Count, categories.Count);
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(1001)]
        [InlineData(1002)]
        [InlineData(1003)]
        public void GetCategory_WithIncorrectIdShouldReturnNotFound(int id)
        {
            _mockRepo.Setup(repo => repo.GetCategory(id))
                .Returns(_dummyData.categoriesList.FirstOrDefault(x => x.Id == id));

            var result = _controller.GetCategory(id);

            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public void GetCategory_InEmptyDatabaseShouldReturnNotFound()
        {
            var result = _controller.GetCategory(5);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetCategory_WithCorrectIdShouldReturnCategoryDtoAndOkResponse(int id)
        {
            _mockRepo.Setup(repo => repo.GetCategory(id))
                .Returns(_dummyData.categoriesList.FirstOrDefault(x => x.Id == id));
            var categoryObj = _dummyData.categoriesList[id];
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(categoryObj))
                .Returns(new CategoryDto()
                {
                    Description = categoryObj.Description,
                    Id = categoryObj.Id,
                    Image = categoryObj.Image,
                    Title = categoryObj.Title
                });

            var result = _controller.GetCategory(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var category = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal(_dummyData.categoriesList[id].Description, category.Description);
        }

        [Theory]
        [InlineData("Ala ma kota")]
        [InlineData("A kot ma ale")]
        [InlineData("R12344")]
        [InlineData("54432")]
        public void GetCategoryByTitle_WithIncorrectTitleShouldReturnNotFound(string title)
        {
            _mockRepo.Setup(repo => repo.GetCategory(title))
                .Returns(_dummyData.categoriesList.FirstOrDefault(x => x.Title == title));

            var result = _controller.GetCategoryByTitle(title);

            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public void GetCategoryByTitle_InEmptyDatabaseShouldReturnNotFound()
        {
            var result = _controller.GetCategoryByTitle("231245");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetCategoryByTitle_WithNullInArgumentShouldReturnBadRequest()
        {
            var result = _controller.GetCategoryByTitle(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("Włosy")]
        [InlineData("Głowa")]
        [InlineData("Nogi")]
        [InlineData("Uszy")]
        public void GetCategoryByTitle_WithCorrectTitleShouldReturnCategoryDtoAndOkResponse(string title)
        {
            var obj = _dummyData.categoriesList.FirstOrDefault(x => x.Title == title);
            _mockRepo.Setup(repo => repo.GetCategory(title))
                .Returns(obj);
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(obj))
                .Returns(new CategoryDto()
                {
                    Description = obj.Description,
                    Id = obj.Id,
                    Image = obj.Image,
                    Title = obj.Title
                });

            var result = _controller.GetCategoryByTitle(title);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var category = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal(_dummyData.categoriesList.FirstOrDefault(x => x.Title == title)?.Description, category.Description);
        }

        [Fact]
        public void CreateCategory_WithNullArgumentShouldReturnBadRequest()
        {
            var result = _controller.CreateCategory(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        public void CreateCategory_IfCategoryWithThisTitleAlreadyExistsShouldReturnBadRequest()
        {
            _mockRepo.Setup(repo => repo.CategoryExists(It.IsAny<string>())).Returns(true);

            var result = _controller.CreateCategory(new CategoryCreateDto()
                {Description = "AAA", Image = null, Title = "Włosy"});

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateCategory_IfModelStateIsNotValidShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("Title","Category doesn't have title");

            var result = _controller.CreateCategory(new CategoryCreateDto()
            {
                Description = "Opis",
                Image = null,
                Title = null
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateCategory_WhenSystemProblemOccuringShouldReturn500ErrorStatus()
        {
            var categoryCreateDtoObj = new CategoryCreateDto()
            {
                Description = "Opis",
                Image = null,
                Title = "Kolczyki"
            };
            var categoryCreateObj = new Category()
            {
                CreationDateTime = DateTime.Now,
                Description = categoryCreateDtoObj.Description,
                Image = null,
                Title = categoryCreateDtoObj.Title
            };
            _mockMapper.Setup(repo => repo.Map<Category>(categoryCreateDtoObj))
                .Returns(categoryCreateObj);
            _mockRepo.Setup(repo => repo.CreateCategory(categoryCreateObj))
                .Returns(false);

            var result = _controller.CreateCategory(categoryCreateDtoObj);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public void CreateCategory_WithCorrectObjectShouldReturnCreatedAtRoute()
        {
            Category emp = null;
            var categoryCreateDtoObj = new CategoryCreateDto()
            {
                Description = "Opis",
                Image = null,
                Title = "Kolczyki"
            };
            var categoryCreateObj = new Category()
            {
                CreationDateTime = DateTime.Now,
                Description = categoryCreateDtoObj.Description,
                Image = null,
                Title = categoryCreateDtoObj.Title,
            };
            _mockMapper.Setup(repo => repo.Map<Category>(categoryCreateDtoObj))
                .Returns(categoryCreateObj);
            _mockRepo.Setup(repo => repo.CreateCategory(categoryCreateObj))
                .Callback<Category>(x => emp = x).Returns(true);
            SetupContextClass();

            var result = _controller.CreateCategory(categoryCreateDtoObj);

            _mockRepo.Verify(x => x.CreateCategory(categoryCreateObj), Times.Once);
            Assert.Equal(emp.Title,categoryCreateObj.Title);
            Assert.Equal(emp.Description,categoryCreateObj.Description);
            Assert.Equal(emp.Image,categoryCreateObj.Image);

        }

        private void SetupContextClass()
        {
            var featureCollection = new Mock<IFeatureCollection>();
            var version = new ApiVersion(1, 0);
            var query = new Mock<IQueryCollection>();
            var request = new Mock<HttpRequest>();
            var httpContext = new Mock<HttpContext>();

            featureCollection.Setup(f => f.Get<IApiVersioningFeature>()).Returns(() => new ApiVersioningFeature(httpContext.Object));
            query.SetupGet(q => q["api-version"]).Returns(new StringValues("42.0"));
            request.SetupGet(r => r.Query).Returns(query.Object);
            httpContext.SetupGet(c => c.Features).Returns(featureCollection.Object);
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.SetupProperty(c => c.RequestServices, Mock.Of<IServiceProvider>());
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Object.HttpContext = httpContext.Object;
            _controller.ControllerContext = controllerContextMock.Object;
        }



    }
}
