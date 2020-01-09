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
        public async Task<List<UserArticle>> UserArticlesOrderByViewCounts(string key)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", key);

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            articles = articles.OrderByDescending(p => p.PageViewsCount).ToList();

            return articles;
        }

        [HttpGet]
        [Route("StatsArticles")]
        public async Task<StatsArticles> StatsArticles(string key)
        {
            var statsArticles = new StatsArticles();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", key);

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

        [HttpGet]
        [Route("MostViewedTag")]
        public async Task<MostViewedTag> MostViewedTag(string key)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", key);

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            articles = articles.OrderByDescending(p => p.TagList.Length).ToList();

            var tags = new Dictionary<string, double>();

            foreach (var article in articles)
            {
                foreach (var tag in article.TagList)
                {
                    if (!tags.ContainsKey(tag))
                        tags.Add(tag, article.PageViewsCount);
                    else
                        tags[tag] = tags[tag] + article.PageViewsCount;
                }
            }

            var dicEntry = tags.OrderByDescending(c => c.Value).FirstOrDefault();
            var mostViewedTag = new MostViewedTag() { Tag = dicEntry.Key, Views = dicEntry.Value };

            return mostViewedTag;
        }

        [HttpGet]
        [Route("MostReactionsTag")]
        public async Task<MostReactionsTag> MostReactionsTag(string key)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", key);

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            articles = articles.OrderByDescending(p => p.TagList.Length).ToList();

            var tags = new Dictionary<string, double>();

            foreach (var article in articles)
            {
                foreach (var tag in article.TagList)
                {
                    if (!tags.ContainsKey(tag))
                        tags.Add(tag, article.PositiveReactionsCount);
                    else
                        tags[tag] = tags[tag] + article.PositiveReactionsCount;
                }
            }

            var dicEntry = tags.OrderByDescending(c => c.Value).FirstOrDefault();
            var mostReactionsTag = new MostReactionsTag() { Tag = dicEntry.Key, Reactions = dicEntry.Value };

            return mostReactionsTag;
        }

        [HttpGet]
        [Route("MostCommentedTag")]
        public async Task<MostCommentedTag> MostCommentedTag(string key)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", key);

            var response = await client.GetStringAsync("https://dev.to/api/articles/me?page=1");
            var articles = JsonConvert.DeserializeObject<List<UserArticle>>(response);

            articles = articles.OrderByDescending(p => p.TagList.Length).ToList();

            var tags = new Dictionary<string, double>();

            foreach (var article in articles)
            {
                foreach (var tag in article.TagList)
                {
                    if (!tags.ContainsKey(tag))
                        tags.Add(tag, article.CommentsCount);
                    else
                        tags[tag] = tags[tag] + article.CommentsCount;
                }
            }

            var dicEntry = tags.OrderByDescending(c => c.Value).FirstOrDefault();
            var mostCommentedTag = new MostCommentedTag() { Tag = dicEntry.Key, Comments = dicEntry.Value };

            return mostCommentedTag;
        }
    }
}
