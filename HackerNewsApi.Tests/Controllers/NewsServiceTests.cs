using HackerNewsApi.Controllers;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsApi.Tests.Controllers
{
    public class NewsServiceTests
    {
        private MockRepository mockRepository;

        private Mock<HttpMessageHandler> mockHttpMessageHandler;

        public NewsServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockHttpMessageHandler = this.mockRepository.Create<HttpMessageHandler>();
        }

        private NewsService CreateService()
        {
            return new NewsService(
                new HttpClient(mockHttpMessageHandler.Object));
        }

        [Fact]
        public async Task GetTopStories_Calls_HttpClient_And_Returns_List_Of_IDs()
        {
            // Arrange
            var service = this.CreateService();
            var testEnum = Enumerable.Range(0, 10).ToList();
            var jsonList = JsonSerializer.Serialize(testEnum);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync" , new object[] {
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()})
                .ReturnsAsync(new HttpResponseMessage() { 
                    Content = new StringContent(jsonList), 
                    StatusCode = System.Net.HttpStatusCode.OK });

            // Act
            var result = await service.GetTopStories();

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(10, resultList.Count);
            Assert.All(resultList, v => Assert.Contains(v, testEnum));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetStory_Calls_HttpClient_And_Returns_Story()
        {
            // Arrange
            var service = this.CreateService();
            var testStory = new Story() {
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
            var jsonStory = JsonSerializer.Serialize(testStory);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", new object[] {
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()})
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(jsonStory),
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            var result = await service.GetStory(
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
    }
}
