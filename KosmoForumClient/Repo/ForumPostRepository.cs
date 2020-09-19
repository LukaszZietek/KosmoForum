﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public override async Task<bool> CreateAsync(string url, ForumPost obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (obj == null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            return false;

        }

        public override async Task<bool> UpdateAsync(string url, int id, ForumPost obj)
        {
            if (obj == null)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, url + id);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj,Formatting.Indented,new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

    }
}
