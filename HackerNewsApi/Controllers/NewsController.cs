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
        private readonly IVotesService votesService;

        public NewsController(INewsService newsService, IVotesService votesService)
        {
            this.newsService = newsService;
            this.votesService = votesService;
        }

        // GET api/values
        [HttpGet("item/{id}.json")]
        public Task<Story> GetStory(int id)
        {
            return newsService.GetStory(id);
        }

        [HttpGet("topstories.json")]
        public Task<IEnumerable<int>> GetTopStories()
        {
            return newsService.GetTopStories();
        }




        [HttpGet("item/{id}/upvote")]
        public IActionResult UpVote(int id)
        {
            var voterID = Request.Cookies["voterID"];
            votesService.UpVote(voterID, id);
            return this.NoContent();
        }

        [HttpGet("item/{id}/downvote")]
        public IActionResult DownVote(int id)
        {
            var voterID = Request.Cookies["voterID"];
            votesService.DownVote(voterID, id);
            return this.NoContent();
        }

        [HttpGet("item/{id}/resetvote")]
        public IActionResult ResetVote(int id)
        {
            var voterID = Request.Cookies["voterID"];
            votesService.ResetVote(voterID, id);
            return this.NoContent();
        }

        [HttpPost("votes")]
        public Dictionary<string, Vote> Votes([FromBody] List<int> ids)
        {
            var voterID = Request.Cookies["voterID"];
            return votesService.GetVotes(ids, voterID).ToDictionary(pair => $"{pair.Key}", pair => pair.Value);
        }

    }
}