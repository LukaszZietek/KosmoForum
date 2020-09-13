using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KosmoForumClient.Repo
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public CategoryRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
