using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

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
        Dictionary<int, Dictionary<string, int>> votes = new Dictionary<int, Dictionary<string, int>>();
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
            storyVotes.Remove(voterID);
        }

        private Dictionary<string, int> GetStoryVotes(int id)
        {
            var hasVotes = votes.ContainsKey(id);

            if (!hasVotes)
            {
                votes.Add(id, new Dictionary<string, int>());
            }
            return votes[id];
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