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
        private NewsService newsClient;

        public CachingNewsService(NewsService newsClient)
        {
            this.newsClient = newsClient;
        }

        private List<string> topstories = new List<string>();
        private Dictionary<string, Story> stories = new Dictionary<string, Story>();

        public async Task<IEnumerable<string>> GetTopStories()
        {
            if (topstories.Count == 0)
            {
                var response = await newsClient.GetTopStories();
                topstories.AddRange(response);

            }
            return topstories;
        }

        public async Task<Story> GetStory(string id)
        {
            if (!stories.ContainsKey(id))
            {
                var response = await newsClient.GetStory(id);
                stories[id] = response;
            }
            return stories[id];
        }
    }

}