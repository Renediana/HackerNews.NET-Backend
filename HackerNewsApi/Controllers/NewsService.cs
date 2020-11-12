using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HackerNewsApi.Controllers
{
    public class NewsService : INewsService
    {
        private HttpClient httpClient;

        public NewsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<int>> GetTopStories()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<int>>("https://hacker-news.firebaseio.com/v0/topstories.json");
        }

        public async Task<Story> GetStory(int id)
        {
            return await httpClient.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
        }


    }

    public interface INewsService
    {
        Task<IEnumerable<int>> GetTopStories();
        Task<Story> GetStory(int id);
    }
}