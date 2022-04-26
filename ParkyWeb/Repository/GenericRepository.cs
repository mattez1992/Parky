using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected HttpClient _client;

        public GenericRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<T> GetAsync(string url, int id, string token)
        {
            if (token != null && token.Length != 0)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _client.GetFromJsonAsync<T>($"{url}/{id}");
            return response;
        }
        public async Task<IEnumerable<T>> GetAllAsync(string url, string token)
        {
            if (token != null && token.Length != 0)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);               
            }
            var response = await _client.GetFromJsonAsync<IEnumerable<T>>(url);
            return response;
        }
        public async Task<bool> AddAsync(string url, T entity, string token)
        {
            if (token != null && token.Length != 0)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var result = await _client.PostAsJsonAsync(url, entity);
            return (result.StatusCode == HttpStatusCode.Created);

        }

        public async Task<bool> UpdateAsync(string url, T updateObject, string token)
        {
            if (token != null && token.Length != 0)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _client.PatchAsync(url, new StringContent(
                    JsonConvert.SerializeObject(updateObject), Encoding.UTF8, "application/json"));
            return (response.StatusCode == HttpStatusCode.NoContent);
        }
        public async Task<bool> Delete(string url, int id, string token)
        {
            if (token != null && token.Length != 0)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _client.DeleteAsync($"{url}/{id}");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
