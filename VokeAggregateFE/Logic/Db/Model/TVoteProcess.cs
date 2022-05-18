using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VokeAggregateFE.Logic.Db.Model
{
    /// <summary>
    /// 投票集計用テーブル
    /// </summary>
    [Table("t_vote_process")]
    public class TVoteProcess
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 集計したtweetId
        /// </summary>
        [Column("tweet_id")]
        public long TweetId { get; set; }

        /// <summary>
        /// 集計した日時
        /// </summary>
        [Column("tweet_date")]
        public DateTime TweetDate { get; set; }

        [Column("is_realtime")]
        public bool IsRealTime { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("reg_date")]
        public DateTime RegDate { get; set; }
    }
}
