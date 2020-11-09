using System.Net.Http;
using System.Net.Http.Headers;

namespace HackerNewsApi
{

    public static class ApiHelper
    {

        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            /*ApiClient = new HttpClient();
            ApiClient.BaseAdress = new Uri("https://hacker-news.firebaseio.com/v0/");
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
        }
    }
}