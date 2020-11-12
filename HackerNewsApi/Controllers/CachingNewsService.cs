using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    public class CachingNewsService : INewsService
    {
        private NewsService newsClient;

        public CachingNewsService(NewsService newsClient)
        {
            this.newsClient = newsClient;
        }

        private List<int> topstories = new List<int>();
        private Dictionary<int, Story> stories = new Dictionary<int, Story>();

        public async Task<IEnumerable<int>> GetTopStories()
        {
            if (topstories.Count == 0)
            {
                var response = await newsClient.GetTopStories();
                topstories.AddRange(response);

            }
            return topstories;
        }

        public async Task<Story> GetStory(int id)
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