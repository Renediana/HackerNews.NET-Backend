using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace HackerNewsApi.Controllers
{

    public interface IVotesService
    {
        void DownVote(string voterID, int id);
        void UpVote(string voterID, int id);
        void ResetVote(string voterID, int id);
        Dictionary<int, Vote> GetVotes(List<int> ids, string voterID);
    }

    public class VotesService : IVotesService
    {
        ConcurrentDictionary<int, ConcurrentDictionary<string, int>> votes = new ConcurrentDictionary<int, ConcurrentDictionary<string, int>>();
        public void UpVote(string voterID, int id)
        {
            var storyVotes = GetStoryVotes(id);
            storyVotes[voterID] = 1;
        }
        public void DownVote(string voterID, int id)
        {
            var storyVotes = GetStoryVotes(id);
            storyVotes[voterID] = -1;
        }
        public void ResetVote(string voterID, int id)
        {
            var storyVotes = GetStoryVotes(id);
            var hasUserVote = storyVotes.ContainsKey(voterID);
            if (!hasUserVote)
            {
                return;
            }
            storyVotes.Remove(voterID, out var _);
        }

        private ConcurrentDictionary<string, int> GetStoryVotes(int id)
        {

            return votes.GetOrAdd(id, new ConcurrentDictionary<string, int>());

        }

        public Dictionary<int, Vote> GetVotes(List<int> ids, string voterID)
        {
            return ids.ToDictionary(id => id, id => BuildVote(id, voterID));
        }

        private Vote BuildVote(int id, string voterID)
        {
            var hasVotes = votes.ContainsKey(id);
            var totalVotes = hasVotes ? votes[id].Values.Sum() : 0;

            var hasVoterVote = hasVotes && votes[id].ContainsKey(voterID);
            int? myVote = hasVoterVote ? votes[id][voterID] : (int?)null;

            return new Vote(totalVotes, myVote);
        }
    }

}