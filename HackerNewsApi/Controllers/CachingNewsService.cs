using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HackerNewsApi.Controllers
{
    public class CachingNewsService : INewsService
    {
        private HttpClient httpClient;
        private List<string> topstories = new List<string>();
        private Dictionary<string, Story> stories = new Dictionary<string, Story>();

        public CachingNewsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetTopStories()
        {
            if (topstories.Count == 0)
            {
                var response = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json");
                var json = await response.Content.ReadAsStringAsync();
                topstories.AddRange(JsonConvert.DeserializeObject<IEnumerable<string>>(json));

            }
            return topstories;
        }

        public async Task<Story> GetStory(string id)
        {
            if (!stories.ContainsKey(id))
            {
                var response = await httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
                var json = await response.Content.ReadAsStringAsync();
                stories[id] = JsonConvert.DeserializeObject<Story>(json);
            }
            return stories[id];
        }
    }

}