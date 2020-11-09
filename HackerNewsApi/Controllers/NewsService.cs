using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HackerNewsApi.Controllers
{
    public class NewsService
    {
        private HttpClient httpClient;

        public NewsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetTopStories()
        {
            var response = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<string>>(json);
        }

        public async Task<Story> GetStory(string id)
        {
            var response = await httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Story>(json);
        }
    }
}