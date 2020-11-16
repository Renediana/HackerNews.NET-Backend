namespace HackerNewsApi.Controllers
{
    public class Vote
    {
        public Vote(int total, int? myVote)
        {
            this.total = total;
            this.myVote = myVote;
        }

        public int total { get; }
        public int? myVote { get; }


    }
}