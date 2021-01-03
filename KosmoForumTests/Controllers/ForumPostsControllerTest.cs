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
using Microsoft.AspNetCore.Mvc;
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
                    Title = cat.Title
                });
        }


    }
}
