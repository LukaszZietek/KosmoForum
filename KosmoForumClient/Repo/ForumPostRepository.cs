using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Newtonsoft.Json;

namespace KosmoForumClient.Repo
{
    public class ForumPostRepository : Repository<ForumPost>, IForumPostRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public ForumPostRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<IEnumerable<ForumPost>> GetAllFromCategory(string url, int categoryId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+"getforumpostincategory/"+ categoryId );
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringObj = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ForumPost>>(stringObj);
            }
            return null;


        }
    }
}
