using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsApi.Controllers
{
    public class CachingNewsService : INewsService
    {
        private NewsService newsClient;

        private DateTime? cachingTime;

        private NewsServiceOptions options;

        public CachingNewsService(NewsService newsClient, NewsServiceOptions options)
        {
            this.newsClient = newsClient;
            this.options = options;
        }

        private List<int> topstories = new List<int>();
        private Dictionary<int, Story> stories = new Dictionary<int, Story>();

        public async Task<IEnumerable<int>> GetTopStories()
        {
            var cacheIsInvalid = !cachingTime.HasValue || DateTime.UtcNow > cachingTime.Value.Add(options.Interval);
            if (cacheIsInvalid)
            {
                if (topstories.Count == 0)
                {
                    var response = await newsClient.GetTopStories();
                    topstories.AddRange(response);

                    cachingTime = DateTime.UtcNow;
                }
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