using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VokeAggregateFE.Logic.Db.Model
{
    /// <summary>
    /// 投票内容
    /// </summary>
    [Table("t_vote_unit")]
    public class TVoteUnit
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ユニット名
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 投票日時(tweetの投稿日時)
        /// </summary>
        [Column("vote_date")]
        public DateTime VoteDate { get; set; }

        /// <summary>
        /// 不明票かどうか
        /// </summary>
        [Column("is_unknown")]
        public bool IsUnknown { get; set; }

        [Column("tweet_id")]
        public long TweetId { get; set; }

        [Column("tweet_text")]
        public string TweetText { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("reg_date")]
        public DateTime RegDate { get; set; }
    }
}
