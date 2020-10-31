using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;

namespace KosmoForumClient.Repo
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public CategoryRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Tuple<string,Category>> GetAsyncByTitle(string url, string title, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+ "GetCategoryByTitle/"+title);
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringObj = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<Category>(stringObj));
            }

            var errorObj = await response.Content.ReadAsStringAsync();
            return Tuple.Create(JsonConvert.DeserializeAnonymousType(errorObj,new {message=""}).message, new Category() {});
        }
    }
}
