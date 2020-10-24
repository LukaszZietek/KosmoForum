using System;
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
                var obj = JsonConvert.DeserializeAnonymousType(jsonString, new {message = ""});
                returnObj = Tuple.Create(obj.message, new User() { });
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
                return Tuple.Create(JsonConvert.DeserializeAnonymousType(jsonString, new {message = ""}).message,
                    false);
            }
        }

        public async Task<int> GetUserId(string url, string username, string token = "")
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
                return JsonConvert.DeserializeObject<int>(stringObj);

            }

            return 0;


        }

        public async Task<byte[]> GetUserAvatar(string url, string token = "")
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
                return JsonConvert.DeserializeObject<byte[]>(stringBytes);
            }

            return null;
        }

        public async Task<bool> UpdateUserAvatar(string url, byte[] avatar, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url+"changeavatar/");
            var client = _clientFactory.CreateClient();

            if (avatar != null && avatar.Length > 0)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(avatar),Encoding.UTF8,"application/json");
            }
            else
            {
                return false;
            }

            if (token != null && token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;



        }

        public async Task<bool> ChangePassword(string url,ChangePasswordVM passwordVM, string token)
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
                return true;
            }

            return false;
        }
    }
}
