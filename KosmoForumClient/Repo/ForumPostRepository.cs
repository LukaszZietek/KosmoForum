using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;

namespace KosmoForumClient.Repo
{
    public class ForumPostRepository : Repository<ForumPost>, IForumPostRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public ForumPostRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
