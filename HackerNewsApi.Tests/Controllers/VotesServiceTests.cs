using HackerNewsApi.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HackerNewsApi.Tests.Controllers
{
    public class VotesServiceTests
    {
        private MockRepository mockRepository;



        public VotesServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private VotesService CreateService()
        {
            return new VotesService();
        }

        [Fact]
        public void UpVote_Assigns_Vote_For_StoryID_At_VoterID()
        {
            // Arrange
            string voterId = "12345";
            int id = 9;
            var votesService = CreateService();  

            // Act
            votesService.UpVote(
                voterId,
                id);

            var testDict = votesService.Votes;
            // Assert
            Assert.True(testDict.ContainsKey(id));
            var votes = testDict[id];
            Assert.True(votes.ContainsKey(voterId));
            Assert.Equal(1, votes[voterId]);
            Assert.Single(votes);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void DownVote_Assigns_Vote_For_StoryID_At_VoterID()
        {
            // Arrange
            string voterId = "12345";
            int id = 9;
            var votesService = CreateService();

            // Act
            votesService.DownVote(
                voterId,
                id);

            var testDict = votesService.Votes;
            // Assert
            Assert.True(testDict.ContainsKey(id));
            var votes = testDict[id];
            Assert.True(votes.ContainsKey(voterId));
            Assert.Equal(-1, votes[voterId]);
            Assert.Single(votes);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void ResetVote_Deletes_Vote_For_StoryID_At_VoterID()
        {
            // Arrange
            string voterId = "12345";
            int id = 9;
            var votesService = CreateService();
            var testDict = votesService.Votes;
            // Act
            testDict[id] = new Dictionary<string, int>()
            {
                { voterId, 1 }
            };

            votesService.ResetVote(
                voterId,
                id);

            
            // Assert
            Assert.True(testDict.ContainsKey(id));
            var votes = testDict[id];
            Assert.False(votes.ContainsKey(voterId));
            Assert.Empty(votes);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetVotes_Creates_Dictionary_With_ID_And_Vote()
        {
            // Arrange
            var service = this.CreateService();
            List<int> ids = Enumerable.Range(0, 10).ToList();
            string voterID = "12345";
            var testDict = CreateService();

            // Act
            var result = service.GetVotes(
                ids,
                voterID);

            // Assert
            Assert.Equal(10, result.Count);
            foreach (var id in ids)
            {
                Assert.True(result.ContainsKey(id));
            }
            this.mockRepository.VerifyAll();
        }
    }
}
