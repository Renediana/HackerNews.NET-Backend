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
        // GET api/values
        [HttpGet("item/{id}.json")]
        public ActionResult<IEnumerable<string>> GetStory( string id )
        {
            
            return new string[] { "12345", id };
        }

        // GET api/values
        [HttpGet("topstories.json")]
        public ActionResult<IEnumerable<string>> GetTopStories()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
