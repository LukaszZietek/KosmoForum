using System;
using System.Collections.Generic;
using System.Text;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;

namespace KosmoForumTests.FakeRepo
{
    public class ForumPostFakeData
    {
        public List<ForumPost> dummyForumPosts;
        public List<ForumPostDto> dummyForumPostsDtos;
        public ForumPostCreateDto dummyForumPostCreateDto;
        public ForumPostUpdateDto dummyForumPostUpdateDto;

        public ForumPostFakeData()
        {
            dummyForumPosts = new List<ForumPost>()
            {
                new ForumPost()
                {
                    Id = 0,
                    CategoryId = 1,
                    Content = "Zawartosc",
                    Date = DateTime.Now,
                    Title = "Tytuł",
                    UserId = 123
                },
                new ForumPost()
                {
                    Id = 1,
                    CategoryId = 1,
                    Content = "Zawartosc forumposta",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta",
                    UserId = 53
                },
                new ForumPost()
                {
                    Id = 2,
                    CategoryId = 2,
                    Content = "Zawartosc forumposta drugiego",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta drugiego",
                    UserId = 123
                },
                new ForumPost()
                {
                    Id = 3,
                    CategoryId = 2,
                    Content = "Zawartosc forumposta trzeciego",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta trzeciego",
                    UserId = 10
                }
            };

            dummyForumPostsDtos = new List<ForumPostDto>();
            MapForumPostToForumPostDto();

            dummyForumPostCreateDto = new ForumPostCreateDto()
            {
                CategoryId = 5,
                Content = "Dwdwdw",
                Title = "Tytul",
                Images = new List<ImageCreateDto>()
            };

            dummyForumPostUpdateDto = new ForumPostUpdateDto()
            {
                Id = 10,
                CategoryId = 7,
                Content = "Dwdwdwd",
                Title = "Tytul",
                UserId = 10,
                Images = new List<ImageDto>()
            };

        }

        private void MapForumPostToForumPostDto()
        {
            for (int i = 0; i < dummyForumPosts.Count; i++)
            {
                dummyForumPostsDtos.Add(new ForumPostDto()
                {
                    Id = dummyForumPosts[i].Id,
                    CategoryId = dummyForumPosts[i].CategoryId,
                    Content = dummyForumPosts[i].Content,
                    Date = dummyForumPosts[i].Date,
                    Title = dummyForumPosts[i].Title,
                    UserId = dummyForumPosts[i].UserId
                });
            }
        }
    }
}
