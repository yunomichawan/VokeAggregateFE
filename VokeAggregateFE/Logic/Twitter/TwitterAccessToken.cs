using CoreTweet;
using System;
using System.Collections.Generic;
using System.Text;

namespace VokeAggregateFE
{
    public class TwitterAccessToken : Singleton<TwitterAccessToken>
    {
        #region 定数

        private const string API_KEY = "************************";
        private const string API_KEY_SECRET = "************************";
        private const string ACCESS_TOKEN = "************************";
        private const string ACCESS_TOKEN_SECRET = "************************";
        private const string BEARER_TOKEN = "************************";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Tokens Tokens { get; set; }

        /// <summary>
        /// 自身のId
        /// </summary>
        public long UserId => 1326317887905177601;

        /// <summary>
        /// 
        /// </summary>
        private TrendLocation JpLocation { get; set; }

        public TwitterAccessToken()
        {
            this.Tokens = Tokens.Create(API_KEY, API_KEY_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
            this.SetJpLocation();
        }

        private void SetJpLocation()
        {
            var locations = this.Tokens.Trends.Available();

            foreach (var location in locations)
            {
                if (location.CountryCode == null)
                    continue;

                if (location.CountryCode.Equals("JP"))
                {
                    this.JpLocation = location;
                    break;
                }
            }
        }

    }
}
