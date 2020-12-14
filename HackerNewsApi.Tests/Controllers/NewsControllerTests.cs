using HackerNewsApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsApi.Tests.Controllers
{
    public class NewsControllerTests
    {
        private MockRepository mockRepository;

        private Mock<INewsService> mockNewsService;
        private Mock<IVotesService> mockVotesService;

        public NewsControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockNewsService = this.mockRepository.Create<INewsService>();
            this.mockVotesService = this.mockRepository.Create<IVotesService>();
        }

        private NewsController CreateNewsController()
        {
            return new NewsController(
                this.mockNewsService.Object,
                this.mockVotesService.Object);
        }

        [Fact]
        public async Task GetStory_Calls_NewsService_And_Returns_Story()
        {
            // Arrange
            var newsController = this.CreateNewsController();
            var testStory = new Story()
            {
                By = "Bonenga",
                Descendants = 100,
                Id = 1024,
                Kids = new int[] { },
                Score = 144,
                Time = 9734798,
                Title = "Test",
                Type = "Story",
                Url = "http://customurl.onion"
            };

            mockNewsService
                .Setup(service => service.GetStory(It.Is<int>(id => id == testStory.Id)))
                .ReturnsAsync(testStory);

            // Act
            var result = await newsController.GetStory(
                testStory.Id);

            // Assert
            Assert.Equal(testStory.By, result.By);
            Assert.Equal(testStory.Descendants, result.Descendants);
            Assert.Equal(testStory.Id, result.Id);
            Assert.Equal(testStory.Kids, result.Kids);
            Assert.Equal(testStory.Score, result.Score);
            Assert.Equal(testStory.Time, result.Time);
            Assert.Equal(testStory.Title, result.Title);
            Assert.Equal(testStory.Type, result.Type);
            Assert.Equal(testStory.Url, result.Url);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetTopStories_Calls_NewsService_And_Returns_List_Of_IDs()
        {
            // Arrange
            var newsController = this.CreateNewsController();
            var testEnum = Enumerable.Range(0, 10);
            var testList = Enumerable.Range(0, 10).ToList();

            mockNewsService
                .Setup(service => service.GetTopStories())
                .ReturnsAsync(testEnum);

            // Act
            var result = await newsController.GetTopStories();

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(10, resultList.Count);
            Assert.All(resultList, v => Assert.Contains(v, testEnum));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void UpVote_Calls_VotesService_UpVote()
        {
            // Arrange
            string voterId = "12345";
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Cookie", $"voterId={voterId}");
            var newsController = this.CreateNewsController();
            newsController.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };
            
            int id = 0;
            

            mockVotesService
                .Setup(service => service.UpVote( 
                    It.Is<string>(v => v == voterId), 
                    It.Is<int>(v => v == id)));

            // Act
            var result = newsController.UpVote(
                id);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void DownVote_Calls_VotesService_DownVote()
        {
            // Arrange
            string voterId = "12345";
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Cookie", $"voterId={voterId}");
            var newsController = this.CreateNewsController();
            newsController.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            int id = 0;


            mockVotesService
                .Setup(service => service.DownVote(
                    It.Is<string>(v => v == voterId),
                    It.Is<int>(v => v == id)));

            // Act
            var result = newsController.DownVote(
                id);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void ResetVote_Calls_VotesService_ResetVote()
        {
            // Arrange
            string voterId = "12345";
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Cookie", $"voterId={voterId}");
            var newsController = this.CreateNewsController();
            newsController.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            int id = 0;


            mockVotesService
                .Setup(service => service.ResetVote(
                    It.Is<string>(v => v == voterId),
                    It.Is<int>(v => v == id)));

            // Act
            var result = newsController.ResetVote(
                id);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Votes_Calls_VotesService_GetVotes()
        {
            // Arrange
            string voterId = "12345";
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Cookie", $"voterId={voterId}");
            var newsController = this.CreateNewsController();
            newsController.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            List<int> ids = Enumerable.Range(0, 10).ToList();
            Vote vote = new Vote(10, 9);

            mockVotesService
                .Setup(service => service.GetVotes(It.IsAny<List<Int32>>(), It.Is<String>(v => v == voterId)))
                .Returns(new Dictionary<int, Vote>() {
                    { 7,vote }
                });

            // Act
            var result = newsController.Votes(
                ids);

            // Assert
            Assert.All( ids,  v => Assert.Contains(v, ids));
            this.mockRepository.VerifyAll();
        }
    }
}
