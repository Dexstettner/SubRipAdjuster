using SubRipAdjuster.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;


namespace SubRipAdjuster.DAL
{
    public class SubripDatabaseContext : DbContext
    {

        public SubripDatabaseContext() : base("SubripDatabase")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SubripDatabaseContext>());
        }
        public DbSet<ArquiveHistory> ArquiveHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}