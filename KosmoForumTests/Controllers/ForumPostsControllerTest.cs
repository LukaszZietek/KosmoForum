using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DeepEqual.Syntax;
using KosmoForum.Controllers;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;
using KosmoForum.Repository;
using KosmoForum.Repository.IRepository;
using KosmoForumTests.FakeRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Primitives;
using Xunit;
using Moq;

namespace KosmoForumTests.Controllers
{
    public class ForumPostsControllerTest
    {
        private readonly Mock<IForumPostRepo> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ForumPostsController _forumPostsController;
        private readonly ForumPostFakeData _dummyData;

        public ForumPostsControllerTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IForumPostRepo>();
            _forumPostsController = new ForumPostsController(_mockMapper.Object, _mockRepo.Object);
            _dummyData = new ForumPostFakeData();
            SetupRepo();
            SetupMapper();
            SetupCorrectAuthorizeContext();
        }

        [Fact]
        public void GetForumPost_WithIncorrectIdShouldReturnNotFoundResponse()
        {
            _mockRepo.Setup(repo => repo.GetPost(100)).Returns((int i) => null);

            var result = _forumPostsController.GetForumPost(100);

            _mockRepo.Verify(x => x.GetPost(It.IsAny<int>()),Times.Once);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetForumPost_WithCorrectIdShouldReturnOkResponseWithObject()
        {
            var result = _forumPostsController.GetForumPost(5);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ForumPostDto>(objectResult.Value);
        }

        [Fact]
        public void GetForumPosts_WithEmptyDatabaseShouldReturnNotFound()
        {
            _mockRepo.Setup(repo => repo.GetAllPosts()).Returns((ICollection<ForumPost>) null);

            var result = _forumPostsController.GetForumPosts();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetForumPosts_ShouldReturnOkResponseWithListsOfDto()
        {
            var result = _forumPostsController.GetForumPosts();

            _mockRepo.Verify(repo => repo.GetAllPosts(),Times.Once);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var listOfItems = Assert.IsType<List<ForumPostDto>>(objectResult.Value);
            _dummyData.dummyForumPostsDtos.ShouldDeepEqual(listOfItems);
        }

        [Fact]
        public void GetForumPostsInCategory_WhenForumPostListBelongsToSpecificCategoryIsEmptyShouldReturnNotFound()
        {
            var result = _forumPostsController.GetForumPostsInCategory(100);

            _mockRepo.Verify(repo => repo.GetAllForumPostsInCategory(It.IsAny<int>()),Times.Once);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public void GetForumPostsInCategory_WhenForumPostsExistShouldReturnListsOfDto(int categoryId)
        {
            var result = _forumPostsController.GetForumPostsInCategory(categoryId);

            _mockRepo.Verify(repo => repo.GetAllForumPostsInCategory(It.IsAny<int>()), Times.Once);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var listObj = Assert.IsType<List<ForumPostDto>>(objectResult.Value);
            listObj.ShouldDeepEqual(_dummyData.dummyForumPostsDtos.Where(x => x.CategoryId == 1));
        }

        [Fact]
        public void GetForumPostsForUser_WhenWrongUserShouldReturnBadRequest()
        {
            SetupWrongAuthorizeContext();

            var result = _forumPostsController.GetForumPostsForUser();

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetForumPostsForUser_WhenUserDoesNotHavePostsShouldReturnNotFound()
        {
            _mockRepo.Setup(x => x.GetAllForumPostsForUser(It.IsAny<int>())).Returns((ICollection<ForumPost>)null);

            var result = _forumPostsController.GetForumPostsForUser();

            _mockRepo.Verify(repo => repo.GetAllForumPostsForUser(123), Times.Once);
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetForumPostsForUser_WithCorrectAuthorizationForUserWhoAlreadyHavePostsShouldReturnOk()
        {
            var result = _forumPostsController.GetForumPostsForUser();

            _mockRepo.Verify(repo => repo.GetAllForumPostsForUser(123), Times.Once);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var listObj = Assert.IsType<List<ForumPostDto>>(objectResult.Value);
            listObj.ShouldDeepEqual(_dummyData.dummyForumPostsDtos.Where( x => x.UserId == 123));
        }

        [Fact]
        public void CreateForumPost_WithNullArgumentShouldReturnBadRequest()
        {
            var result = _forumPostsController.CreateForumPost(null);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateForumPost_WhenForumPostWithThisNameAlreadyExistsShouldReturnNotFound()
        {
            _mockRepo.Setup(repo => repo.ForumPostIfExist(It.IsAny<string>())).Returns(true);

            var result = _forumPostsController.CreateForumPost(_dummyData.dummyForumPostCreateDto);

            _mockRepo.Verify(repo => repo.ForumPostIfExist("Tytul"), Times.Once);
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CreateForumPost_WhenPassedArgumentIsNotValidShouldReturnBadRequest()
        {
            _forumPostsController.ModelState.AddModelError("Title", "Forum post doesn't have title");

            var result = _forumPostsController.CreateForumPost(_dummyData.dummyForumPostCreateDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateForumPost_WhenCategoryDoesNotExistShouldReturnBadRequest()
        {
            _mockRepo.Setup(repo => repo.CategoryIfExists(It.IsAny<int>())).Returns(false);

            var result = _forumPostsController.CreateForumPost(_dummyData.dummyForumPostCreateDto);
            
            _mockRepo.Verify(repo => repo.CategoryIfExists(_dummyData.dummyForumPostCreateDto.CategoryId), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateForumPost_WhenSystemErrorOccuredShouldReturn500StatusCode()
        {
            _mockRepo.Setup(repo => repo.CreateForumPost(It.IsAny<ForumPost>())).Returns(false);

            var result = _forumPostsController.CreateForumPost(_dummyData.dummyForumPostCreateDto);


            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError,objectResult.StatusCode);
        }

        [Fact]
        public void CreateForumPost_WhenEverythingIsFineShouldReturn201StatusCode()
        {
            SetupContextClass();

            var result = _forumPostsController.CreateForumPost(_dummyData.dummyForumPostCreateDto);

            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.IsType<ForumPost>(objectResult.Value);
        }

        [Fact]
        public void UpdateForumPost_WhenPassedObjIsNullShouldReturnBadRequest()
        {
            var result = _forumPostsController.UpdateForumPost(5, null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateForumPost_WhenPassedObjHasDifferentIdThanThisPassedToTheRequestShouldReturnBadRequest()
        {
            var result = _forumPostsController.UpdateForumPost(15, _dummyData.dummyForumPostUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateForumPost_WhenNewCategoryIsNotExist()
        {
            _mockRepo.Setup(repo => repo.CategoryIfExists(It.IsAny<int>())).Returns(false);

            var result = _forumPostsController.UpdateForumPost(10, _dummyData.dummyForumPostUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateForumPost_WhenModelStateIsNotValidShouldReturnBadRequest()
        {
            _forumPostsController.ModelState.AddModelError("Title", "Title has wrong value");

            var result = _forumPostsController.UpdateForumPost(10, _dummyData.dummyForumPostUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateForumPost_WhenNewTitleAlreadyExistsInDatabaseShouldReturnBadRequest()
        {
            _mockRepo.Setup(repo => repo.GetPost(It.IsAny<string>())).Returns(_dummyData.dummyForumPosts[0]);

            var result = _forumPostsController.UpdateForumPost(10, _dummyData.dummyForumPostUpdateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateForumPost_WhenServerErrorOcureShouldReturn500StatusCode()
        {
            _mockRepo.Setup(repo => repo.UpdateForumPost(It.IsAny<ForumPost>())).Returns(false);

            var result = _forumPostsController.UpdateForumPost(10, _dummyData.dummyForumPostUpdateDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public void UpdateForumPost_WhenEverythingIsOkShouldReturnNoContent()
        {
            var result = _forumPostsController.UpdateForumPost(10, _dummyData.dummyForumPostUpdateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        public void DeleteForumPost_WhenForumPostDoesNotExistShouldReturnBadRequest(int value)
        {
            _mockRepo.Setup(repo => repo.ForumPostIfExist(value)).Returns(false);

            var result = _forumPostsController.DeleteForumPost(value);

            _mockRepo.Verify(repo => repo.ForumPostIfExist(value), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteForumPost_WhenServerErrorOccuredShouldReturn500StatusCode()
        {
            _mockRepo.Setup(repo => repo.DeleteForumPost(It.IsAny<ForumPost>())).Returns(false);

            var result = _forumPostsController.DeleteForumPost(10);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public void DeleteForumPost_WhenEverythingFineShouldReturnNoContent()
        {
            var result = _forumPostsController.DeleteForumPost(10);

            Assert.IsType<NoContentResult>(result);
        }





        private void SetupRepo()
        {
            _mockRepo.Setup(repo => repo.GetPost(It.IsAny<int>())).Returns((int i) => new ForumPost
            {
                CategoryId = 0,
                Content = "Ala ma kota",
                Date = DateTime.Now,
                Id = i,
                Title = "Tytul"
            });

            _mockRepo.Setup(repo => repo.GetAllPosts()).Returns(_dummyData.dummyForumPosts);
            _mockRepo.Setup(repo => repo.GetAllForumPostsInCategory(It.IsAny<int>()))
                .Returns((int i) =>
                {
                    var value = _dummyData.dummyForumPosts.Where(x => x.CategoryId == i).ToList();
                    return value.Count > 0 ? value : null;
                });
            _mockRepo.Setup(x => x.GetAllForumPostsForUser(It.IsAny<int>())).Returns((int i) =>
            {
                return _dummyData.dummyForumPosts.Where(forumpost => forumpost.UserId == i).ToList();
            });
            _mockRepo.Setup(repo => repo.ForumPostIfExist(It.IsAny<string>())).Returns(false);
            _mockRepo.Setup(repo => repo.ForumPostIfExist(It.IsAny<int>())).Returns(true);
            _mockRepo.Setup(repo => repo.CreateForumPost(It.IsAny<ForumPost>())).Returns(true);
            _mockRepo.Setup(repo => repo.CategoryIfExists(It.IsAny<int>())).Returns(true);
            _mockRepo.Setup(repo => repo.GetPost(It.IsAny<string>())).Returns((ForumPost) null);
            _mockRepo.Setup(repo => repo.UpdateForumPost(It.IsAny<ForumPost>())).Returns(true);
            _mockRepo.Setup(repo => repo.DeleteForumPost(It.IsAny<ForumPost>())).Returns(true);
        }

        private void SetupMapper()
        {
            _mockMapper.Setup(repo => repo.Map<ForumPostDto>(It.IsAny<ForumPost>())).Returns((ForumPost cat) =>
                new ForumPostDto()
                {
                    Id = cat.Id,
                    CategoryId = cat.CategoryId,
                    Content = cat.Content,
                    Date = cat.Date,
                    Title = cat.Title,
                    UserId = cat.UserId
                });
            _mockMapper.Setup(repo => repo.Map<ForumPost>(It.IsAny<ForumPostCreateDto>())).Returns( (ForumPostCreateDto forum) =>
                new ForumPost()
                {
                    Title = forum.Title,
                    Content = forum.Content,
                    UserId = forum.UserId,
                    CategoryId = forum.CategoryId,
                    Images = new List<Image>(forum.Images.Select(imageObject => new Image()
                    {
                        Picture = imageObject.Picture,
                        UserId = imageObject.UserId
                    }).ToList())
                });
            _mockMapper.Setup(repo => repo.Map<ForumPost>(It.IsAny<ForumPostUpdateDto>())).Returns(
                (ForumPostUpdateDto updateValue) => new ForumPost()
                {
                    Id = updateValue.Id,
                    Title = updateValue.Title,
                    Content = updateValue.Content,
                    UserId = updateValue.UserId,
                    CategoryId = updateValue.CategoryId,
                    Images = new List<Image>(updateValue.Images.Select(value => new Image()
                    {
                        Id = value.Id,
                        Picture = value.Picture
                    }))
                });
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
            _forumPostsController.ControllerContext = controllerContextMock.Object;
        }

        private void SetupWrongAuthorizeContext()
        {
            var httpcontextMock = new Mock<HttpContext>();
            httpcontextMock.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);
            httpcontextMock.SetupGet(x => x.User.Identity.Name).Returns("");
            _forumPostsController.ControllerContext.HttpContext = httpcontextMock.Object;
        }

        private void SetupCorrectAuthorizeContext()
        {
            var httpcontextMock = new Mock<HttpContext>();
            httpcontextMock.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(true);
            httpcontextMock.SetupGet(x => x.User.Identity.Name).Returns("123");
            _forumPostsController.ControllerContext.HttpContext = httpcontextMock.Object;
        }


    }
}
