﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace KosmoForumClient.Repo
{
    public class AccountRepository : Repository<User>,IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Tuple<string,User>> LoginAsync(string url, User objToCreate)
        {
            Tuple<string, User> returnObj = Tuple.Create(" ",new User(){});

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate),Encoding.UTF8,"application/json");
            }
            else
            {
                return returnObj;
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                returnObj = Tuple.Create("", JsonConvert.DeserializeObject<User>(jsonString));
                return returnObj;
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                returnObj = Tuple.Create(ModelStateDeserializer.DeserializeModelState(jsonString), new User() { });
                return returnObj;
            }

        }

        public async Task<Tuple<string, bool>> RegisterAsync(string url, User objToCreate)
        {

            var request = new HttpRequestMessage(HttpMethod.Post,url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate),Encoding.UTF8,"application/json");
            }
            else
            {
                return Tuple.Create("Object to create shouldn't be null",false);
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Tuple.Create("", true);
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return Tuple.Create(ModelStateDeserializer.DeserializeModelState(jsonString),
                    false);
            }
        }

        public async Task<Tuple<string,int>> GetUserId(string url, string username, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (username != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(username),Encoding.UTF8,"application/json");
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringObj = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<int>(stringObj));

            }

            var jsonString = await response.Content.ReadAsStringAsync();

            return Tuple.Create(
                ModelStateDeserializer.DeserializeModelState(jsonString),
                0);


        }

        public async Task<Tuple<string,byte[]>> GetUserAvatar(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+"getuseravatar/");
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringBytes = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<byte[]>(stringBytes));
            }
            var stringBytesContent = await response.Content.ReadAsStringAsync();
            return Tuple.Create<string,byte[]>(
               ModelStateDeserializer.DeserializeModelState(stringBytesContent),
                null);

        }

        public async Task<Tuple<string,bool>> UpdateUserAvatar(string url, byte[] avatar, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url+"changeavatar/");
            var client = _clientFactory.CreateClient();

            if (avatar != null && avatar.Length > 0)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(avatar),Encoding.UTF8,"application/json");
            }
            else
            {
                return Tuple.Create("Problem occurred during processing file",false);
            }

            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return Tuple.Create("", true);
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(jsonString),false);



        }

        public async Task<Tuple<string,bool>> ChangePassword(string url,ChangePasswordVM passwordVM, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url+"changepassword/");
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(passwordVM),Encoding.UTF8,"application/json");
            

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return Tuple.Create("", true);
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(jsonString), false);
        }
    }
}
