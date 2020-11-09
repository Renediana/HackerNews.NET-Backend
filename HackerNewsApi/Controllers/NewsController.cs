using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsService newsService;

        public NewsController(NewsService newsService)
        {
            this.newsService = newsService;
        }

        // GET api/values
        [HttpGet("item/{id}.json")]
        public Task<Story> GetStory(string id)
        {
            return newsService.GetStory(id);
        }

        // GET api/values
        [HttpGet("topstories.json")]
        public Task<IEnumerable<string>> GetTopStories()
        {
            return newsService.GetTopStories();
        }
    }
}
