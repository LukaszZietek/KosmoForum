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
                    Title = "Tytuł"
                },
                new ForumPost()
                {
                    Id = 1,
                    CategoryId = 1,
                    Content = "Zawartosc forumposta",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta"
                },
                new ForumPost()
                {
                    Id = 2,
                    CategoryId = 2,
                    Content = "Zawartosc forumposta drugiego",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta drugiego"
                },
                new ForumPost()
                {
                    Id = 3,
                    CategoryId = 2,
                    Content = "Zawartosc forumposta trzeciego",
                    Date = DateTime.Now,
                    Title = "Tytuł forumposta trzeciego"
                }
            };

            dummyForumPostsDtos = new List<ForumPostDto>();
            MapForumPostToForumPostDto();
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
                    Title = dummyForumPosts[i].Title
                });
            }
        }
    }
}
