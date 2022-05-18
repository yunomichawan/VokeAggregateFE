using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using VokeAggregateFE.Logic.Twitter.Aggregate;

namespace VokeAggregateFE
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tweets = TwitterAccessToken.Instance.Tokens.Statuses.UserTimeline(user_id => "FE_Heroes_JP",screen_name=> "FE_Heroes_JP", count => 10);

            //foreach(var tweet in tweets)
            //{
            //    Console.WriteLine($"TweetId：{tweet.Id}");
            //    Console.WriteLine($"Texr：{tweet.Text}");
            //}
            Console.WriteLine("realtime:0,past:1");
            string val = Console.ReadLine();

            TweetAggregate aggregate = new TweetAggregate();
            //aggregate.Run();
            // 1分毎に起動するイベント
            Timer timer = new Timer(1000 * 60);
            timer.Elapsed += (sender, e) =>
            {
                aggregate.Run(val);
            };
            timer.Start();
            Console.WriteLine("Hello World!");
            Console.WriteLine("終了したくなったらEnter");
            Console.ReadLine();
        }
    }
}
