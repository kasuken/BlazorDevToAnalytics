using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorDevToAnalytics.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorDevToAnalytics.Server.Controllers
{
    [Route("api/[controller]")]
    public class DevToAnalyticsController : Controller
    {
        [HttpGet]
        public async Task<List<PublishedArticle>> Get()
        {
            var articles = new List<PublishedArticle>();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", "");

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            articles = JsonConvert.DeserializeObject<List<PublishedArticle>>(response);

            return articles;
        }
    }
}
