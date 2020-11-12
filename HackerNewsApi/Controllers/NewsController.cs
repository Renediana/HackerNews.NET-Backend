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
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        // GET api/values
        [HttpGet("item/{id}.json")]
        public Task<Story> GetStory(int id)
        {
            return newsService.GetStory(id);
        }

        // GET api/values
        [HttpGet("topstories.json")]
        public Task<IEnumerable<int>> GetTopStories()
        {
            return newsService.GetTopStories();
        }
    }
}
