﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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


        public async Task<Tuple<string,IEnumerable<ForumPost>>> GetAllFromCategory(string url, int categoryId, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+"getforumpostsincategory/"+ categoryId );
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringObj = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<IEnumerable<ForumPost>>(stringObj));
            }

            var errorString = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorString), Enumerable.Empty<ForumPost>());


        }

        public async Task<Tuple<string,IEnumerable<ForumPost>>> GetAllBelongsToUser(string url, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+"getforumpostsforuser");
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string obj = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<IEnumerable<ForumPost>>(obj));
            }

            string errorObj = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorObj),
                Enumerable.Empty<ForumPost>());
        }

        public override async Task<Tuple<string,bool>> CreateAsync(string url, ForumPost obj, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (obj == null)
            {
                return Tuple.Create("Object which u send is empty", false);
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Tuple.Create("",true);
            }

            var errorResponse = await response.Content.ReadAsStringAsync();

            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorResponse), false);

        }

        public override async Task<Tuple<string,bool>> UpdateAsync(string url, int id, ForumPost obj, string token = "")
        {
            if (obj == null)
            {
                return Tuple.Create("Object which you want send to database is empty", false);
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, url + id);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj,Formatting.Indented,new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return Tuple.Create("", true);
            }

            var errorString = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorString), false);
        }

    }
}
