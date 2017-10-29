using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SMS_ass1.Models
{
    public class PGDbContext : DbContext
    {
        public PGDbContext() : base(nameOrConnectionString: "DefaultConnectionString") { }
        public virtual DbSet<Task> tasks { get; set; }

        //public System.Data.Entity.DbSet<SMS_ass1.Models.Task> tasks { get; set; }
    }
}
