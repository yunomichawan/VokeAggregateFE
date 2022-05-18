using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VokeAggregateFE.Logic.Db.Model;

namespace VokeAggregateFE.Logic.Db
{
    public class VoteEntities : DbContext
    {
        public DbSet<TVoteUnit> VokeUnits { get; set; }

        public DbSet<TVoteProcess> VokeProcess { get; set; }

        public VoteEntities()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(Properties.Resources.ConnectionString);
        }
    }
}
