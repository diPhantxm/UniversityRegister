using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class UniAPI
    {
        private readonly HttpClient _client;

        public UniAPI(string uri)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(uri),
                MaxResponseContentBufferSize = 32768
            };
        }

        public async Task<ActionResult<T>> Get<T>(string uri)
        {
            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return new NotFoundResult();
            }

            var responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "[]" || string.IsNullOrWhiteSpace(responseString))
            {
                return new NotFoundResult();
            }
            var result = JsonConvert.DeserializeObject<T>(responseString);

            return result;
        }

        public async Task<ActionResult<TResult>> Post<TResult, TValue>(string uri, TValue content)
        {
            var json = JsonConvert.SerializeObject(content);
            var response = await _client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                return new EmptyResult();
            }
            
            var responseString = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResult>(responseString);

            return new ActionResult<TResult>(result);
        }

        public void SetJWT(Jwt jwt)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Token);
        }

        public void SetJWT(string jwt)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }
}
