using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VokeAggregateFE.Logic.Db;
using VokeAggregateFE.Logic.Db.Model;
using System.Text.RegularExpressions;
using CoreTweet;

namespace VokeAggregateFE.Logic.Twitter.Aggregate
{
    public class TweetAggregate
    {
        #region readonly,const

        private long START_TWEET_ID = 1352450979539640324;

        /// <summary>
        /// 集計タグ
        /// </summary>
        private const string AGGREGATE_TAG = "#FE英雄総選挙";

        /// <summary>
        /// 集計開始日時
        /// </summary>
        private readonly DateTime StartDate = DateTime.Parse("2021/1/21 12:00");

        private const string FE_ACCOUNT = "FE_Heroes_JP";

        #endregion

        #region property

        /// <summary>
        /// 集計用データ
        /// </summary>
        private TVoteProcess VoteProcess { get; set; }

        /// <summary>
        /// twiiter access token
        /// </summary>
        private TwitterAccessToken Token => TwitterAccessToken.Instance;

        private VoteEntities VoteEntities { get; set; }

        #endregion

        public TweetAggregate()
        {
            this.VoteEntities = new VoteEntities();
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Run(string value)
        {
            if (value.Equals("0"))
            {
                this.RealTimeVote();
            }
            else
            {
                this.PastVote();
            }
        }

        private void RealTimeVote()
        {

            this.VoteProcess = this.VoteEntities
                .VokeProcess.Where(v => v.IsRealTime)
                .OrderByDescending(v => v.Id).FirstOrDefault();
            long sinceId = this.VoteProcess == null ? START_TWEET_ID : this.VoteProcess.TweetId;


            var tweets = Token.Tokens.Search.Tweets(q => AGGREGATE_TAG
                            , count => 100
                            , since_id => sinceId
                            , result_type => "recent"
                            //, until => StartDate.ToString("yyyy-MM-dd")
                            , lang => "ja"
                            , locale => "ja");

            this.AddVotes(tweets, tweet => tweet.Id > sinceId);

            var lastTweet = tweets.OrderByDescending(t => t.Id).First();
            this.VoteEntities.VokeProcess.Add(
                new TVoteProcess
                {
                    TweetId = lastTweet.Id,
                    TweetDate = lastTweet.CreatedAt.DateTime,
                    IsRealTime = true,
                    RegDate = DateTime.Now
                });
            this.VoteEntities.SaveChanges();
        }

        private void PastVote()
        {
            this.VoteProcess = this.VoteEntities
                .VokeProcess.Where(v => !v.IsRealTime)
                .OrderByDescending(v => v.Id).FirstOrDefault();
            long maxId = this.VoteProcess == null ? START_TWEET_ID : this.VoteProcess.TweetId;


            var tweets = Token.Tokens.Search.Tweets(q => AGGREGATE_TAG
                            , count => 100
                            , max_id => maxId
                            , result_type => "recent"
                            //, until => StartDate.ToString("yyyy-MM-dd")
                            , lang => "ja"
                            , locale => "ja");

            this.AddVotes(tweets, tweet => tweet.Id < maxId && tweet.CreatedAt.DateTime > DateTime.Parse("2021/01/26 16:17:59"));

            var lastTweet = tweets.OrderBy(t => t.Id).First();
            this.VoteEntities.VokeProcess.Add(
                new TVoteProcess
                {
                    TweetId = lastTweet.Id,
                    TweetDate = lastTweet.CreatedAt.DateTime,
                    IsRealTime = false,
                    RegDate = DateTime.Now
                });
            this.VoteEntities.SaveChanges();
        }

        private void AddVotes(SearchResult tweets, Func<Status, bool> isTarget)
        {
            if (tweets.Count < 1)
            {
                // 処理終了
                Console.WriteLine("************");
                Console.WriteLine("取得できたtweetなし。");
                Console.WriteLine("************");
                return;
            }

            foreach (var tweet in tweets)
            {
                if (Regex.IsMatch(tweet.Text, "「.*」") && isTarget(tweet))
                {
                    this.AddVote(tweet);
                }
            }
        }

        /// <summary>
        /// 投票追加
        /// </summary>
        /// <param name="status"></param>
        private void AddVote(Status status)
        {
            string unitName = this.GetUnitName(status.Text);
            TVoteUnit unit = new TVoteUnit()
            {
                Name = unitName,
                VoteDate = status.CreatedAt.DateTime,
                RegDate = DateTime.Now,
                TweetId = status.Id,
                TweetText = status.Text,
                IsUnknown = false
            };
            this.OutputTweet(status);
            this.VoteEntities.VokeUnits.Add(unit);
        }

        private string GetUnitName(string tweet)
        {
            MatchCollection mc = Regex.Matches(tweet, @"「.*」");
            foreach (Match m in mc)
            {
                return m.Value.Substring(1, m.Value.Length - 2);
            }

            return null;
        }

        private void OutputTweet(Status status)
        {
            Console.WriteLine("*************");
            Console.WriteLine($"Id:{status.Id}");
            Console.WriteLine($"Text:{status.Text}");
            Console.WriteLine("");
            Console.WriteLine($"FullText:{status.FullText}");
            Console.WriteLine("*************");
        }
    }
}
