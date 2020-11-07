using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KosmoForumClient.Repo.IRepo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KosmoForumClient.Repo
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Tuple<string,T>> GetAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<T>(jsonString));
            }

            string errorString = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorString),
                default(T));
        }

        public async Task<Tuple<string,IEnumerable<T>>> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var obj = await response.Content.ReadAsStringAsync();
                return Tuple.Create("", JsonConvert.DeserializeObject<IEnumerable<T>>(obj));
            }

            var errorStr = await response.Content.ReadAsStringAsync();
            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorStr), Enumerable.Empty<T>());
        }

        public virtual async Task<Tuple<string,bool>> UpdateAsync(string url, int id, T obj, string token = "")
        {
            if (obj == null)
            {
                return Tuple.Create("Object which you want send to database is empty", false);
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, url+id);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj),Encoding.UTF8,"application/json");

            var client = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return Tuple.Create("", true);
            }

            var errorStr = await response.Content.ReadAsStringAsync();

            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorStr), false);
        }

        public async Task<Tuple<string,bool>> DeleteAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);

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

            var errorStr = await response.Content.ReadAsStringAsync();

            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(errorStr), false);
        }

        public virtual async Task<Tuple<string,bool>> CreateAsync(string url, T obj, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (obj == null)
            {
                return Tuple.Create("Object which u want send is null",false);
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj),Encoding.UTF8,"application/json");

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

            string responseError = await response.Content.ReadAsStringAsync();


            return Tuple.Create(ModelStateDeserializer.DeserializeModelState(responseError), false);

            


        }
    }
}
