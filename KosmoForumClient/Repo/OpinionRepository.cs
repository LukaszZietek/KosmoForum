using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace KosmoForumClient.Repo
{
    public class OpinionRepository : Repository<Opinion> , IOpinionRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public OpinionRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Tuple<string,IEnumerable<Opinion>>> GetUserOpinion(string url, string token = "")
        {
            var obj = await GetAllAsync(url + "getusersopinion", token);
            return obj;
        }
    }
}
