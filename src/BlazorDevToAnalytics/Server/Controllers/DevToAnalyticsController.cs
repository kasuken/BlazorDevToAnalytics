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
    public class DevToAnalyticsController : ControllerBase
    {
        [HttpGet]
        [Route("UserArticlesOrderByViewCounts")]
        public async Task<List<UserArticle>> UserArticlesOrderByViewCounts()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", "");

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            articles = articles.OrderByDescending(p => p.PageViewsCount).ToList();

            return articles;
        }

        [HttpGet]
        [Route("StatsArticles")]
        public async Task<StatsArticles> StatsArticles()
        {
            var statsArticles = new StatsArticles();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", "");

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            foreach (var item in articles)
            {
                statsArticles.TotalViews += item.PageViewsCount;
                statsArticles.TotalComments += item.CommentsCount;
                statsArticles.TotalReactions += item.PositiveReactionsCount;
            }

            statsArticles.TotalArticles = articles.Count;

            return statsArticles;
        }
    }
}
